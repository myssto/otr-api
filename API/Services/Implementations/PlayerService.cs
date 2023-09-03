using API.Configurations;
using API.Entities;
using API.Services.Interfaces;
using Dapper;
using Microsoft.Identity.Client;
using Npgsql;

namespace API.Services.Implementations;

public class PlayerService : ServiceBase<Player>, IPlayerService
{
	private readonly IServiceProvider _serviceProvider;
	public PlayerService(ICredentials credentials, ILogger<PlayerService> logger, IServiceProvider serviceProvider) : base(credentials, logger)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task<Player?> GetByOsuIdAsync(long osuId)
	{
		using (var connection = new NpgsqlConnection(ConnectionString))
		{
			var player = await connection.QuerySingleOrDefaultAsync<Player>("SELECT * FROM players WHERE osu_id = @OsuId", new { OsuId = osuId });
			
			if (player == null)
			{
				return null;
			}

			using (var scope = _serviceProvider.CreateScope())
			{
				var matchesService = scope.ServiceProvider.GetRequiredService<IMatchesService>();
				var ratingsService = scope.ServiceProvider.GetRequiredService<IRatingsService>();
				var historyService = scope.ServiceProvider.GetRequiredService<IRatingHistoryService>();
				var webHistoryService = scope.ServiceProvider.GetRequiredService<IUserService>();
				
				var matchData = await matchesService.GetForPlayerAsync(player.Id);
				var ratings = await ratingsService.GetForPlayerAsync(player.Id);
				var ratingHistories = await historyService.GetForPlayerAsync(player.Id);
				var webInfo = await webHistoryService.GetForPlayerAsync(player.Id);
				
				player.Matches = matchData.ToList();
				player.Ratings = ratings.ToList();
				player.RatingHistories = ratingHistories.ToList();
				player.WebInfo = webInfo;
			}
			

			return player;
		}
	}

	// public async Task<IEnumerable<Player>> GetByOsuIdsAsync(IEnumerable<long> osuIds)
	// {
	// 	using (var connection = new NpgsqlConnection(ConnectionString))
	// 	{
	// 		return await connection.QueryAsync<Player>("SELECT * FROM players WHERE osu_id = ANY(@OsuIds)", new { OsuIds = osuIds });
	// 	}
	// }

	public async Task<IEnumerable<Player>> GetByOsuIdsAsync(IEnumerable<long> osuIds)
	{
		using(var scope = _serviceProvider.CreateScope())
		using (var connection = new NpgsqlConnection(ConnectionString))
		{
			var matchesService = scope.ServiceProvider.GetRequiredService<IMatchesService>();
			const string sql = @"SELECT * FROM players p WHERE osu_id = ANY(@OsuIds)";
			
			var players = (await connection.QueryAsync<Player>(sql, new { OsuIds = osuIds })).ToList();
			foreach (var p in players)
			{
				var matchData = await matchesService.GetForPlayerAsync(p.Id);
				p.Matches = matchData.ToList();
				
				// TODO: Get ratings and ratinghistories
			}

			return players;
		}
	}

	public async Task<int> GetIdByOsuIdAsync(long osuId)
	{
		using (var connection = new NpgsqlConnection(ConnectionString))
		{
			return await connection.QuerySingleOrDefaultAsync<int>("SELECT id FROM players WHERE osu_id = @OsuId", new { OsuId = osuId });
		}
	}

	public async Task<long> GetOsuIdByIdAsync(int id)
	{
		using (var connection = new NpgsqlConnection(ConnectionString))
		{
			return await connection.QuerySingleOrDefaultAsync<long>("SELECT osu_id FROM players WHERE id = @Id", new { Id = id });
		}
	}

	public async Task<Dictionary<long, int>> GetIdsByOsuIdsAsync(IEnumerable<long> osuIds)
	{
		using (var connection = new NpgsqlConnection(ConnectionString))
		{
			var results = await connection.QueryAsync<(long osuId, int id)>("SELECT osu_id, id FROM players WHERE osu_id = ANY(@OsuIds)", new { OsuIds = osuIds.ToArray() });
			return results.ToDictionary(x => x.osuId, x => x.id);
		}
	}
}