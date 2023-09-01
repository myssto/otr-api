using API.Entities;

namespace API.Services.Interfaces;

public interface IPlayerService : IService<Player>
{
	Task<Player?> GetByOsuIdAsync(long osuId);
	Task<IEnumerable<Player>> GetByOsuIdAsync(IEnumerable<long> osuIds);
	Task<int> GetIdByOsuIdAsync(long osuId);
	Task<long> GetOsuIdByIdAsync(int id);
}