using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoviCode.Commands.Players;
using NoviCode.Queries.Players;

namespace NoviCode.Api;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
	private readonly IMediator _mediator;

	public PlayersController(IMediator mediator) => _mediator = mediator;

	// POST /players — create a player. Caching + logging are applied by decorators, not here.
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken cancellationToken)
	{
		Guid id;
		try
		{
			id = await _mediator.Send(new CreatePlayerCommand(request.Name, request.Score), cancellationToken);
		}
		catch (ArgumentException ex)
		{
			// Empty name / negative score → 400.
			return BadRequest(new { error = ex.Message });
		}

		return Created($"/players/{id}", null);
	}

	// GET /players/{id} — 200 or 404.
	[HttpGet("{id:guid}", Name = "GetPlayerById")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var player = await _mediator.Send(new GetPlayerByIdQuery(id), cancellationToken);
		return player is null ? NotFound() : Ok(PlayerResponse.From(player));
	}

	// GET /players — list all players.
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var players = await _mediator.Send(new GetAllPlayersQuery(), cancellationToken);
		return Ok(players.Select(PlayerResponse.From));
	}
}
