using API.DTOs;
using API.Entities;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class RatingsService : ServiceBase<Rating>, IRatingsService
{
	private readonly OtrContext _context;
	private readonly ILogger<RatingsService> _logger;
	private readonly IMapper _mapper;

	public RatingsService(ILogger<RatingsService> logger, IMapper mapper, OtrContext context) : base(logger, context)
	{
		_logger = logger;
		_mapper = mapper;
		_context = context;
	}

	public async Task<IEnumerable<RatingDTO>> GetForPlayerAsync(long osuPlayerId)
	{
		int dbId = await _context.Players.Where(x => x.OsuId == osuPlayerId).Select(x => x.Id).FirstOrDefaultAsync();

		if (dbId == 0)
		{
			return new List<RatingDTO>();
		}

		return _mapper.Map<IEnumerable<RatingDTO>>(await _context.Ratings.Where(x => x.PlayerId == dbId).ToListAsync());
	}

	public override async Task<int> UpdateAsync(Rating entity)
	{
		// First, copy the current state of the entity to the history table.
		var history = new RatingHistory
		{
			PlayerId = entity.PlayerId,
			Mu = entity.Mu,
			Sigma = entity.Sigma,
			Created = DateTime.UtcNow,
			Mode = entity.Mode
		};

		await _context.RatingHistories.AddAsync(history);
		return await base.UpdateAsync(entity);
	}

	public async Task<int> InsertOrUpdateForPlayerAsync(int playerId, Rating rating)
	{
		var existingRating = await _context.Ratings
		                                   .Where(r => r.PlayerId == rating.PlayerId && r.Mode == rating.Mode)
		                                   .FirstOrDefaultAsync();

		if (existingRating != null)
		{
			existingRating.Mu = rating.Mu;
			existingRating.Sigma = rating.Sigma;
			existingRating.Updated = rating.Updated;
		}
		else
		{
			_context.Ratings.Add(rating);
		}

		return await _context.SaveChangesAsync();
	}

	public async Task<int> BatchInsertAsync(IEnumerable<RatingDTO> ratings)
	{
		var ls = new List<Rating>();
		foreach (var rating in ratings)
		{
			ls.Add(new Rating
			{
				PlayerId = rating.PlayerId,
				Mu = rating.Mu,
				Sigma = rating.Sigma,
				Created = DateTime.UtcNow,
				Mode = rating.Mode
			});
		}

		await _context.Ratings.AddRangeAsync(ls);
		return await _context.SaveChangesAsync();
	}

	public async Task<IEnumerable<RatingDTO>> GetAllAsync() => _mapper.Map<IEnumerable<RatingDTO>>(await _context.Ratings.ToListAsync());
	public async Task TruncateAsync() => await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE ratings RESTART IDENTITY;");
}