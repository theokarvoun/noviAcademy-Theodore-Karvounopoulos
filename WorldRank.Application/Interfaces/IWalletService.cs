using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;

namespace WorldRank.Application.Interfaces;

/// <summary>
/// Application use-cases for wallets. Coordinates the repository and the funds strategies
/// and lets domain exceptions bubble up. Performs no presentation I/O.
/// </summary>
public interface IWalletService
{
	Wallet AddWallet(int playerId, Currency currency, decimal initialBalance);

	IReadOnlyList<Wallet> GetWalletsOfPlayer(int playerId);

	Wallet GetWallet(int playerId, Currency currency);

	void Deposit(int playerId, Currency currency, decimal amount);

	void Withdraw(int playerId, Currency currency, decimal amount);

	void Block(int playerId, Currency currency);

	void Unblock(int playerId, Currency currency);

	void UpdateBalance(int playerId, Currency currency, decimal newBalance);

	void ApplyFundsOperation(int playerId, Currency currency, FundsOperation operation, decimal amount);
}
