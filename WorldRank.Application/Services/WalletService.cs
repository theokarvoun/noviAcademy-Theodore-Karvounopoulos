using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Application.Services;

/// <summary>
/// Application use-cases for wallets. Pure orchestration: it takes plain inputs,
/// coordinates the repository and the funds strategies, and lets domain exceptions
/// bubble up. It performs no console I/O - presentation is the delivery mechanism's job.
///
/// Day 6: the service owns the cache. Reads are cache-aside; every write goes to storage
/// first and then invalidates the affected cache entries so the next read rebuilds them.
/// </summary>
public class WalletService : IWalletService
{
	private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

	private readonly IWalletRepository _walletRepository;
	private readonly IPlayerRepository _playerRepository;
	private readonly ICache _cache;
	private readonly ILogger<WalletService> _logger;
	private readonly IReadOnlyDictionary<FundsOperation, IFundsStrategy> _fundsStrategies;

	public WalletService(
		IWalletRepository walletRepository,
		IPlayerRepository playerRepository,
		IEnumerable<IFundsStrategy> strategies,
		ICache cache,
		ILogger<WalletService> logger)
	{
		_walletRepository = walletRepository;
		_playerRepository = playerRepository;
		_cache = cache;
		_logger = logger;

		// Index every registered strategy by the operation it implements.
		_fundsStrategies = strategies.ToDictionary(strategy => strategy.Operation);
	}

	// Cache keys: one entry per (player, currency) wallet, plus the per-player list view.
	private static string WalletKey(int playerId, Currency currency) => $"wallet:{playerId}:{currency}";
	private static string PlayerWalletsKey(int playerId) => $"wallets:player:{playerId}";

	public Wallet AddWallet(int playerId, Currency currency, decimal initialBalance)
	{
		if (_playerRepository.FindPlayer(playerId) is null)
			throw new PlayerNotFoundException(playerId);

		var wallet = new Wallet(GenerateWalletId(), playerId, currency, initialBalance);
		_walletRepository.Add(wallet); // storage first
		Refresh(wallet);               // then update the cache
		return wallet;
	}

	public IReadOnlyList<Wallet> GetWalletsOfPlayer(int playerId)
	{
		if (_cache.TryGet(PlayerWalletsKey(playerId), out IReadOnlyList<Wallet>? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT  wallets of player {PlayerId}", playerId);
			return cached;
		}

		_logger.LogInformation("Cache MISS wallets of player {PlayerId} — loading from repository", playerId);
		var wallets = _walletRepository.GetAllWalletsByPlayerId(playerId);
		_cache.Set(PlayerWalletsKey(playerId), (IReadOnlyList<Wallet>)wallets, Ttl);
		return wallets;
	}

	public Wallet GetWallet(int playerId, Currency currency)
	{
		if (_cache.TryGet(WalletKey(playerId, currency), out Wallet? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT  wallet {PlayerId} {Currency}", playerId, currency);
			return cached;
		}

		_logger.LogInformation("Cache MISS wallet {PlayerId} {Currency} — loading from repository", playerId, currency);
		var wallet = _walletRepository.GetWallet(playerId, currency); // throws if not found
		_cache.Set(WalletKey(playerId, currency), wallet, Ttl);
		return wallet;
	}

	public void Deposit(int playerId, Currency currency, decimal amount)
	{
		_walletRepository.Deposit(playerId, currency, amount);
		Invalidate(playerId, currency);
	}

	public void Withdraw(int playerId, Currency currency, decimal amount)
	{
		_walletRepository.Withdraw(playerId, currency, amount);
		Invalidate(playerId, currency);
	}

	public void Block(int playerId, Currency currency)
	{
		_walletRepository.Block(playerId, currency);
		Invalidate(playerId, currency);
	}

	public void Unblock(int playerId, Currency currency)
	{
		_walletRepository.Unblock(playerId, currency);
		Invalidate(playerId, currency);
	}

	public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
	{
		_walletRepository.UpdateBalance(playerId, currency, newBalance);
		Invalidate(playerId, currency);
	}

	public void ApplyFundsOperation(int playerId, Currency currency, FundsOperation operation, decimal amount)
	{
		// Pick the strategy that matches the chosen operation (resolved from DI, no factory).
		var strategy = _fundsStrategies[operation];

		var wallet = _walletRepository.GetWallet(playerId, currency);
		strategy.Execute(wallet, amount);

		_logger.LogInformation("Applied {Strategy} of {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
			strategy.GetType().Name, amount, playerId, currency, wallet.Balance);

		Refresh(wallet);
	}

	private int GenerateWalletId()
	{
		var existingIds = _walletRepository.GetAll().Select(wallet => wallet.Id).ToHashSet();

		int id;
		do
		{
			id = Random.Shared.Next(1, int.MaxValue);
		}
		while (existingIds.Contains(id));

		return id;
	}

	// Write-through: store the fresh wallet under its own key and drop the per-player list
	// view so the next read rebuilds it from storage.
	private void Refresh(Wallet wallet)
	{
		_cache.Set(WalletKey(wallet.PlayerId, wallet.Currency), wallet, Ttl);
		_cache.Remove(PlayerWalletsKey(wallet.PlayerId));
		_logger.LogInformation("Cache write-through wallet {PlayerId} {Currency}; list cache invalidated",
			wallet.PlayerId, wallet.Currency);
	}

	// Invalidate both the single-wallet entry and the per-player list after a mutation.
	private void Invalidate(int playerId, Currency currency)
	{
		_cache.Remove(WalletKey(playerId, currency));
		_cache.Remove(PlayerWalletsKey(playerId));
		_logger.LogInformation("Cache invalidated wallet {PlayerId} {Currency}", playerId, currency);
	}
}
