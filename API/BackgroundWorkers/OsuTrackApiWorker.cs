using API.Entities;
using API.Osu;
using API.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace API.BackgroundWorkers;

public class OsuTrackApiWorker : BackgroundService
{
	private const string URL_BASE = "https://osutrack-api.ameo.dev";
	private const int UPDATE_INTERVAL_MINUTES = 1; // Every minute, check if this call needs to be made.
	private const int API_RATELIMIT = 200;         // 200 requests per minute.
	private readonly ILogger<OsuTrackApiWorker> _logger;
	private readonly SemaphoreSlim _semaphore = new(1);
	private readonly IServiceProvider _serviceProvider;
	private DateTime _ratelimitReset = DateTime.UtcNow;
	private int _ratelimitTracker;
	private readonly bool _allowDataFetching;
	private readonly OsuEnums.Mode[] _modes = [OsuEnums.Mode.Standard, OsuEnums.Mode.Taiko, OsuEnums.Mode.Catch, OsuEnums.Mode.Mania];

	public OsuTrackApiWorker(ILogger<OsuTrackApiWorker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_allowDataFetching = configuration.GetValue<bool>("Osu:AutoUpdateUsers");
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (_allowDataFetching)
		{
			await BackgroundTask(stoppingToken);
		}
		else
		{
			_logger.LogInformation("Skipping osu!Track API worker due to configuration");
		}
	}

	private async Task BackgroundTask(CancellationToken stoppingToken)
	{
		var client = new HttpClient();
		_logger.LogInformation("Initialized osu!track API worker");
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await _semaphore.WaitAsync(stoppingToken);
				using (var scope = _serviceProvider.CreateScope())
				{
					var playerService = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
					var ratingStatsRepository = scope.ServiceProvider.GetRequiredService<IMatchRatingStatsRepository>();

					var playersToUpdate = (await playerService.GetPlayersWhereMissingGlobalRankAsync()).ToList();

					if (!playersToUpdate.Any())
					{
						await Task.Delay(UPDATE_INTERVAL_MINUTES * 60 * 1000, stoppingToken);
						continue;
					}

					_logger.LogInformation("Identified {PlayerCount} players to update earliest ranks for", playersToUpdate.Count);
					foreach (var player in playersToUpdate)
					{
						// If the player doesn't have any history, we need to set their earliest mode rank to what we have now,
						// then save what we end up with after we process all the modes.
						SetDefaultEarliestKnownRanks(player);
						await UpdateEarliestKnownRanksAsync(stoppingToken, ratingStatsRepository, player, client);

						_logger.LogInformation("Updated earliest known ranks for player {PlayerId}", player.OsuId);
						await playerService.UpdateAsync(player);
					}
				}
			}
			catch (TaskCanceledException)
			{
				// Ignore, application is probably shutting down
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error occurred while fetching osu!track data");
			}
			finally
			{
				_semaphore.Release();
			}
		}
	}

	private async Task UpdateEarliestKnownRanksAsync(CancellationToken stoppingToken, IMatchRatingStatsRepository ratingStatsRepository, Player player, HttpClient client)
	{
		foreach (var mode in _modes)
		{
			var oldestStatDate = await ratingStatsRepository.GetOldestForPlayerAsync(player.Id, (int)mode);

			if (!oldestStatDate.HasValue)
			{
				continue;
			}

			if (IsRatelimited())
			{
				_logger.LogDebug("osu!track API ratelimited, waiting...");
				await Task.Delay(_ratelimitReset - DateTime.UtcNow, stoppingToken);
			}

			string responseText = await FetchOsuTrackRankAsync(stoppingToken, player, client, mode, oldestStatDate.Value);

			if (string.IsNullOrEmpty(responseText) || responseText == "[]")
			{
				// Nothing found for this player in this mode.
				continue;
			}

			try
			{
				var stats = JsonConvert.DeserializeObject<OsuTrackHistoryStats[]>(responseText);

				if (stats == null)
				{
					continue;
				}

				var relevant = stats[0]; // The response is ordered by date.
				switch (mode)
				{
					case OsuEnums.Mode.Standard:
						player.EarliestOsuGlobalRank = relevant.Rank;
						player.EarliestOsuGlobalRankDate = relevant.Timestamp;
						break;
					case OsuEnums.Mode.Taiko:
						player.EarliestTaikoGlobalRank = relevant.Rank;
						player.EarliestTaikoGlobalRankDate = relevant.Timestamp;
						break;
					case OsuEnums.Mode.Catch:
						player.EarliestCatchGlobalRank = relevant.Rank;
						player.EarliestCatchGlobalRankDate = relevant.Timestamp;
						break;
					case OsuEnums.Mode.Mania:
						player.EarliestManiaGlobalRank = relevant.Rank;
						player.EarliestManiaGlobalRankDate = relevant.Timestamp;
						break;
				}

				_logger.LogInformation("Updated osu!track data for player {PlayerId} in mode {GameMode}, ratelimit = {Ratelimit}: {@Data}", player.OsuId, mode,
					_ratelimitTracker, relevant);
			}
			catch (JsonSerializationException e)
			{
				_logger.LogError(e, "Failed to deserialize osu!track data for player {PlayerId} in mode {GameMode}", player.OsuId, mode);
			}
		}
	}

	private async Task<string> FetchOsuTrackRankAsync(CancellationToken stoppingToken, Player player, HttpClient client, OsuEnums.Mode mode,
		DateTime oldestStatDate)
	{
		string url = FormedUrl(player.OsuId, mode, oldestStatDate, oldestStatDate.AddYears(1));
		var response = await client.GetAsync(url, stoppingToken);
		_ratelimitTracker++;

		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError("Failed to fetch osu!Track history for player {PlayerId} in mode {GameMode}, ratelimit = {Ratelimit}", player.OsuId, mode,
				_ratelimitTracker);

			return string.Empty;
		}

		string responseText = await response.Content.ReadAsStringAsync(stoppingToken);
		return responseText;
	}

	private static void SetDefaultEarliestKnownRanks(Player player)
	{
		if (player.EarliestOsuGlobalRank == null)
		{
			player.EarliestOsuGlobalRank = player.RankStandard;
			player.EarliestOsuGlobalRankDate = DateTime.UtcNow;
		}

		if (player.EarliestTaikoGlobalRank == null)
		{
			player.EarliestTaikoGlobalRank = player.RankTaiko;
			player.EarliestTaikoGlobalRankDate = DateTime.UtcNow;
		}

		if (player.EarliestCatchGlobalRank == null)
		{
			player.EarliestCatchGlobalRank = player.RankCatch;
			player.EarliestCatchGlobalRankDate = DateTime.UtcNow;
		}

		if (player.EarliestManiaGlobalRank == null)
		{
			player.EarliestManiaGlobalRank = player.RankMania;
			player.EarliestManiaGlobalRankDate = DateTime.UtcNow;
		}
	}

	private bool IsRatelimited()
	{
		if (DateTime.UtcNow > _ratelimitReset)
		{
			_ratelimitTracker = 0;
			_ratelimitReset = DateTime.UtcNow.AddMinutes(UPDATE_INTERVAL_MINUTES);
			_logger.LogDebug("osu!Track ratelimiter reset");
		}

		return _ratelimitTracker >= API_RATELIMIT;
	}

	private string FormedUrl(long osuPlayerId, OsuEnums.Mode mode, DateTime from, DateTime? to = null) => new StringBuilder(URL_BASE)
	                                                                                                      .Append("/stats_history")
	                                                                                                      .Append($"?user={osuPlayerId}")
	                                                                                                      .Append($"&mode={(int)mode}")
	                                                                                                      .Append($"&from={from:yyyy-MM-dd}")
	                                                                                                      .Append(to != null ? $"&to={to:yyyy-MM-dd}" : "")
	                                                                                                      .ToString();
}