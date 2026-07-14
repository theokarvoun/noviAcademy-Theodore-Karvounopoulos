using Microsoft.AspNetCore.Mvc;
using WorldRank.API.DTOs;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.API.Controllers;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
	private readonly IPlayerService _players;

	public PlayersController(IPlayerService players) => _players = players;

	// POST /players — create a player (the service writes through to the cache).
	[HttpPost]
	public IActionResult Create([FromBody] CreatePlayerRequest request)
	{
		Player player;
		try
		{
			player = _players.AddPlayer(request.Name, request.Score);
		}
		catch (ArgumentException ex)
		{
			// Empty name / negative score → 400.
			return BadRequest(new { error = ex.Message });
		}

		return CreatedAtAction(nameof(GetById), new { id = player.Id }, PlayerResponse.From(player));
	}

	// GET /players/{id} — 200 or 404 (caching handled by the service).
	[HttpGet("{id:int}")]
	public IActionResult GetById(int id)
	{
		var player = _players.FindPlayerById(id);
		return player is null ? NotFound() : Ok(PlayerResponse.From(player));
	}

	// GET /players — list all players (cached).
	[HttpGet]
	public IActionResult GetAll()
	{
		var players = _players.GetAllPlayers();
		return Ok(players.Select(PlayerResponse.From));
	}
}
