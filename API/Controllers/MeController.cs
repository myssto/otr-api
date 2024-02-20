using API.DTOs;
using API.Services.Interfaces;
using API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers;

[ApiController]
[EnableCors]
[Authorize(Roles = "user")]
[Route("api/[controller]")]
public class MeController : Controller
{
	private readonly IUserService _userService;
	private readonly IPlayerStatsService _playerStatsService;

	public MeController(IUserService userService, IPlayerStatsService playerStatsService)
	{
		_userService = userService;
		_playerStatsService = playerStatsService;
	}

	// Will remove once all users are whitelisted in the database
	// private bool IsWhitelisted(long osuId)
	// {
	// 	var whitelist = new long[]
	// 	{
	// 		11482346,
	// 		8191845,
	// 		11557554,
	// 		4001304,
	// 		6892711,
	// 		7153533,
	// 		3958619,
	// 		6701656,
	// 		1797189,
	// 		7802400,
	// 		11255340,
	// 		13175102,
	// 		11955929,
	// 		11292327
	// 	};
	//
	// 	return whitelist.Contains(osuId);
	// }
	
	[HttpGet]
	[EndpointSummary("Gets the logged in user's information, if they exist")]
	[ProducesResponseType<UserInfoDTO>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetLoggedInUserAsync()
	{
		int? id = HttpContext.AuthorizedUserIdentity();

		if (!id.HasValue)
		{
			return BadRequest("User is not logged in.");
		}

		var user = await _userService.GetAsync(id.Value);
		if (user?.OsuId == null)
		{
			return NotFound("User not found");
		}

		return Ok(user);
	}

	/// <summary>
	///  Validates the currently logged in user's OTR-Access-Token cookie
	/// </summary>
	/// <returns></returns>
	[HttpGet("validate")]
	[Authorize(Roles = "whitelist")]
	[EndpointSummary("Validates the currently logged in user's OTR-Access-Token cookie")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ValidateJwt()
	{
		// Middleware will return 403 if the user does not
		// have the correct roles
		return Ok();
	}

	private int? GetId()
	{
		string? id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss)?.Value;
		if (id == null)
		{
			return null;
		}

		if (!int.TryParse(id, out int idInt))
		{
			return null;
		}

		return idInt;
	}

	[HttpGet("stats")]
	public async Task<ActionResult<PlayerStatsDTO>> GetStatsAsync([FromQuery] int mode = 0, [FromQuery] DateTime? dateMin = null, [FromQuery] DateTime? dateMax = null)
	{
		int? id = GetId();

		if (!id.HasValue)
		{
			return BadRequest("User is not logged in or id could not be retreived from logged in user.");
		}

		return await _playerStatsService.GetAsync(id.Value, null, mode, dateMin ?? DateTime.MinValue, dateMax ?? DateTime.UtcNow);
	}
}