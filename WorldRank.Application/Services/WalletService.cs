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
/// </summary>
public class WalletService
{
	private readonly IWalletRepository _walletRepository;
	private readonly IPlayerRepository _playerRepository;
	private readonly ILogger<WalletService> _logger;
	private readonly IReadOnlyDictionary<FundsOperation, IFundsStrategy> _fundsStrategies;

	public WalletService(
		IWalletRepository walletRepository,
		IPlayerRepository playerRepository,
		IEnumerable<IFundsStrategy> strategies,
		ILogger<WalletService> logger)
	{
		_walletRepository = walletRepository;
		_playerRepository = playerRepository;
		_logger = logger;

		// Index every registered strategy by the operation it implements.
		_fundsStrategies = strategies.ToDictionary(strategy => strategy.Operation);
	}

	public Wallet AddWallet(int playerId, Currency currency, decimal initialBalance)
	{
		if (_playerRepository.FindPlayer(playerId) is null)
			throw new PlayerNotFoundException(playerId);

		var wallet = new Wallet(GenerateWalletId(), playerId, currency, initialBalance);
		_walletRepository.Add(wallet);
		return wallet;
	}

	public IReadOnlyList<Wallet> GetWalletsOfPlayer(int playerId)
	{
		return _walletRepository.GetAllWalletsByPlayerId(playerId);
	}

	public void Deposit(int playerId, Currency currency, decimal amount)
	{
		_walletRepository.Deposit(playerId, currency, amount);
	}

	public void Withdraw(int playerId, Currency currency, decimal amount)
	{
		_walletRepository.Withdraw(playerId, currency, amount);
	}

	public void Block(int playerId, Currency currency)
	{
		_walletRepository.Block(playerId, currency);
	}

	public void Unblock(int playerId, Currency currency)
	{
		_walletRepository.Unblock(playerId, currency);
	}

	public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
	{
		_walletRepository.UpdateBalance(playerId, currency, newBalance);
	}

	public void ApplyFundsOperation(int playerId, Currency currency, FundsOperation operation, decimal amount)
	{
		// Pick the strategy that matches the chosen operation (resolved from DI, no factory).
		var strategy = _fundsStrategies[operation];

		var wallet = _walletRepository.GetWallet(playerId, currency);
		strategy.Execute(wallet, amount);

		_logger.LogInformation("Applied {Strategy} of {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
			strategy.GetType().Name, amount, playerId, currency, wallet.Balance);
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
}
