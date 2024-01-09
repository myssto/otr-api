using API.DTOs;
using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Repositories.Interfaces;

public interface IBaseStatsRepository : IRepository<BaseStats>
{
	/// <summary>
	///  Returns all ratings for a player, one for each game mode (if available)
	/// </summary>
	/// <param name="playerId"></param>
	/// <returns></returns>
	Task<IEnumerable<BaseStats>> GetForPlayerAsync(long osuPlayerId);

	Task<BaseStats?> GetForPlayerAsync(int playerId, int mode);
	Task<int> InsertOrUpdateForPlayerAsync(int playerId, BaseStats baseStats);
	Task<int> BatchInsertAsync(IEnumerable<BaseStats> baseStats);
	Task<IEnumerable<BaseStats>> GetAllAsync();
	Task TruncateAsync();
	Task<int> GetGlobalRankAsync(long osuPlayerId, int mode);

	/// <summary>
	///  Returns the creation date of the most recently created rating entry for a player
	/// </summary>
	/// <returns></returns>
	Task<DateTime> GetRecentCreatedDate(long osuPlayerId);

	Task<IEnumerable<BaseStats>> GetLeaderboardAsync(int page, int pageSize, int mode, LeaderboardChartType chartType,
		LeaderboardFilterDTO? filter, int? playerId);

	Task<int> LeaderboardCountAsync(int requestQueryMode, LeaderboardChartType requestQueryChartType, LeaderboardFilterDTO requestQueryFilter, int? playerId);

	/// <summary>
	///  The highest numeric (aka the worst) rank of a player in our system
	/// </summary>
	/// <param name="country"></param>
	/// <returns></returns>
	Task<int> HighestRankAsync(int mode, string? country = null);

	Task<double> HighestRatingAsync(int mode, string? country = null);
	Task<int> HighestMatchesAsync(int mode, string? country = null);
	Task<ActionResult<IEnumerable<double>>> GetHistogramAsync(int mode);
}