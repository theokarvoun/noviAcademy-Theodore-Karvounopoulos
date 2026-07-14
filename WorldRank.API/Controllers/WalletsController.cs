using Microsoft.AspNetCore.Mvc;
using WorldRank.API.DTOs;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.API.Controllers;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
	private readonly IWalletService _wallets;

	public WalletsController(IWalletService wallets) => _wallets = wallets;

	// POST /wallets — create a wallet for a player in a currency.
	[HttpPost]
	public IActionResult Create([FromBody] CreateWalletRequest request)
	{
		try
		{
			var wallet = _wallets.AddWallet(request.PlayerId, request.Currency, request.InitialBalance);
			return CreatedAtAction(nameof(GetByPlayerAndCurrency),
				new { playerId = wallet.PlayerId, currency = wallet.Currency },
				WalletResponse.From(wallet));
		}
		catch (PlayerNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}
		catch (DuplicateWalletException ex)
		{
			return Conflict(new { error = ex.Message });
		}
		catch (WalletException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
	}

	// GET /wallets/player/{playerId} — all wallets of a player (cached).
	[HttpGet("player/{playerId:int}")]
	public IActionResult GetByPlayer(int playerId)
	{
		var wallets = _wallets.GetWalletsOfPlayer(playerId);
		return Ok(wallets.Select(WalletResponse.From));
	}

	// GET /wallets/{playerId}/{currency} — a single wallet (cached), 404 if missing.
	[HttpGet("{playerId:int}/{currency}")]
	public IActionResult GetByPlayerAndCurrency(int playerId, Currency currency)
	{
		try
		{
			var wallet = _wallets.GetWallet(playerId, currency);
			return Ok(WalletResponse.From(wallet));
		}
		catch (WalletNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}
	}

	// POST /wallets/{playerId}/{currency}/deposit — deposit funds; the service refreshes the cache.
	[HttpPost("{playerId:int}/{currency}/deposit")]
	public IActionResult Deposit(int playerId, Currency currency, [FromBody] AmountRequest request) =>
		Mutate(() => _wallets.Deposit(playerId, currency, request.Amount), playerId, currency);

	// POST /wallets/{playerId}/{currency}/withdraw — withdraw funds.
	[HttpPost("{playerId:int}/{currency}/withdraw")]
	public IActionResult Withdraw(int playerId, Currency currency, [FromBody] AmountRequest request) =>
		Mutate(() => _wallets.Withdraw(playerId, currency, request.Amount), playerId, currency);

	// POST /wallets/{playerId}/{currency}/block — block the wallet.
	[HttpPost("{playerId:int}/{currency}/block")]
	public IActionResult Block(int playerId, Currency currency) =>
		Mutate(() => _wallets.Block(playerId, currency), playerId, currency);

	// POST /wallets/{playerId}/{currency}/unblock — unblock the wallet.
	[HttpPost("{playerId:int}/{currency}/unblock")]
	public IActionResult Unblock(int playerId, Currency currency) =>
		Mutate(() => _wallets.Unblock(playerId, currency), playerId, currency);

	// Runs a mutating use-case, maps domain errors to status codes, then returns the fresh wallet.
	private IActionResult Mutate(Action action, int playerId, Currency currency)
	{
		try
		{
			action();
			return Ok(WalletResponse.From(_wallets.GetWallet(playerId, currency)));
		}
		catch (WalletNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}
		catch (WalletException ex)
		{
			// Blocked wallet, non-positive amount, insufficient funds → 400.
			return BadRequest(new { error = ex.Message });
		}
	}
}
