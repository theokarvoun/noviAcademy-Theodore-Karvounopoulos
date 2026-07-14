using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoviCode.Commands.Wallets;
using NoviCode.Queries.Wallets;

namespace NoviCode.Api;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
	private readonly IMediator _mediator;

	public WalletsController(IMediator mediator) => _mediator = mediator;

	// POST /wallets — create a wallet, return 201 Created with a Location header.
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken cancellationToken)
	{
		var id = await _mediator.Send(new CreateWalletCommand(request.PlayerId, request.Currency), cancellationToken);
		return Created($"/wallets/{id}", null);
	}

	// GET /wallets/{id} — 200 or 404.
	[HttpGet("{id:guid}", Name = "GetWalletById")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var wallet = await _mediator.Send(new GetWalletByIdQuery(id), cancellationToken);
		return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
	}

	// GET /wallets — list all wallets.
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var wallets = await _mediator.Send(new GetAllWalletsQuery(), cancellationToken);
		return Ok(wallets.Select(WalletResponse.From));
	}

	// GET /wallets/by-player/{playerId} — a player's wallets.
	[HttpGet("by-player/{playerId:guid}")]
	public async Task<IActionResult> GetByPlayer(Guid playerId, CancellationToken cancellationToken)
	{
		var wallets = await _mediator.Send(new GetWalletsByPlayerQuery(playerId), cancellationToken);
		return Ok(wallets.Select(WalletResponse.From));
	}

	// POST /wallets/{id}/deposit — deposit funds.
	[HttpPost("{id:guid}/deposit")]
	public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var wallet = await _mediator.Send(new DepositCommand(id, request.Amount), cancellationToken);
			return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
		}
		catch (WalletException ex)
		{
			// Business-rule violation (blocked wallet, non-positive amount) → 400.
			return BadRequest(new { error = ex.Message });
		}
	}

	// POST /wallets/{id}/apply — apply a fund operation (add / subtract / force-subtract).
	[HttpPost("{id:guid}/apply")]
	public async Task<IActionResult> ApplyFunds(Guid id, [FromBody] ApplyFundsRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var wallet = await _mediator.Send(new ApplyFundsCommand(id, request.Amount, request.Operation), cancellationToken);
			return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
		}
		catch (WalletException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
	}

	// POST /wallets/{id}/blocked — block or unblock a wallet.
	[HttpPost("{id:guid}/blocked")]
	public async Task<IActionResult> SetBlocked(Guid id, [FromBody] SetBlockedRequest request, CancellationToken cancellationToken)
	{
		var wallet = await _mediator.Send(new SetWalletBlockedCommand(id, request.Blocked), cancellationToken);
		return wallet is null ? NotFound() : Ok(WalletResponse.From(wallet));
	}
}
