using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : CrudController<Rating>
{
	private readonly IRatingsService _service;
	public RatingsController(ILogger<RatingsController> logger, IRatingsService service) : base(logger, service) { _service = service; }

	[HttpGet("{playerId:int}")]
	public async Task<ActionResult<Rating>> GetForPlayerAsync(int playerId)
	{
		var data = await _service.GetForPlayerAsync(playerId);
		if (data != null)
		{
			return Ok(data);
		}
		
		return NotFound($"User with id {playerId} does not have any data");
	}

	[HttpPut("{playerId:int}/update")]
	public async Task<ActionResult> UpdateForPlayerAsync(int playerId, [FromBody] Rating rating)
	{
		if (playerId != rating.PlayerId)
		{
			return BadRequest($"Player id {rating.PlayerId} in body does not match player id {playerId} in path");
		}
		int? result = await _service.InsertOrUpdateForPlayerAsync(playerId, rating);
		if (result > 0)
		{
			return Ok();
		}

		return StatusCode(500, $"Failed to update rating for player {playerId}");
	}
	
	[HttpPost("batch")]
	public async Task<ActionResult> BatchInsertOrUpdateAsync([FromBody] IEnumerable<Rating> ratings)
	{
		int? result = await _service.BatchInsertOrUpdateAsync(ratings);
		if (result > 0)
		{
			return Ok();
		}

		return StatusCode(500, $"Failed to update ratings");
	}
}