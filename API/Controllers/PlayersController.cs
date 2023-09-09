using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : Controller
{
	private readonly ILogger<PlayersController> _logger;
	private readonly IPlayerService _service;
	public PlayersController(ILogger<PlayersController> logger, IPlayerService service)
	{
		_logger = logger;
		_service = service;
	}
	
	[HttpGet("all")]
	public async Task<ActionResult<IEnumerable<Player>?>> GetAllAsync()
	{
		var players = await _service.GetAllAsync();
		return Ok(players);
	}

	[HttpGet("{osuId:int}")]
	public async Task<ActionResult<Player?>> Get(int osuId)
	{
		var data = await _service.GetByOsuIdAsync(osuId);
		if (data != null)
		{
			return Ok(data);
		}

		return NotFound($"User with id {osuId} does not exist");
	}

	[HttpGet("{osuId:int}/id")]
	public async Task<ActionResult<int>> GetIdByOsuIdAsync(int osuId)
	{
		int id = await _service.GetIdByOsuIdAsync(osuId);
		if (id != 0)
		{
			return Ok(id);
		}

		return NotFound($"User with id {osuId} does not exist");
	}
	
	[HttpGet("{id}/osuid")]
	public async Task<ActionResult<long>> GetOsuIdByIdAsync(int id)
	{
		long osuId = await _service.GetOsuIdByIdAsync(id);
		if (osuId != 0)
		{
			return Ok(osuId);
		}

		return NotFound($"User with id {id} does not exist");
	}
}