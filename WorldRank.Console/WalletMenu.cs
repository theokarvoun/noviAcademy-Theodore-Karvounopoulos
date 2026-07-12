using WorldRank.Application.Services;

namespace WorldRank.Console;

/// <summary>
/// Presentation handlers for the wallet menu. All console I/O lives here; the handlers
/// gather input, call the application service, and render the result. Domain exceptions
/// thrown by the service bubble up to the central handler in Program.
/// </summary>
public class WalletMenu
{
	private readonly WalletService _walletService;

	public WalletMenu(WalletService walletService)
	{
		_walletService = walletService;
	}

	public void AddWalletToPlayer()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var balance = Prompts.PromptAmount("Initial balance");
		if (balance is null)
			return;

		_walletService.AddWallet(playerId.Value, currency.Value, balance.Value);
		System.Console.WriteLine("Wallet added successfully.");
	}

	public void GetWalletsOfPlayer()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var wallets = _walletService.GetWalletsOfPlayer(playerId.Value);

		if (wallets.Count == 0)
		{
			System.Console.WriteLine("No wallets found for this player.");
			return;
		}

		for (var index = 0; index < wallets.Count; index++)
			System.Console.WriteLine($"Wallet Number {index} {wallets[index]}");
	}

	public void DepositToWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var amount = Prompts.PromptAmount("Amount to deposit");
		if (amount is null)
			return;

		_walletService.Deposit(playerId.Value, currency.Value, amount.Value);
		System.Console.WriteLine("Deposit successful.");
	}

	public void WithdrawFromWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var amount = Prompts.PromptAmount("Amount to withdraw");
		if (amount is null)
			return;

		_walletService.Withdraw(playerId.Value, currency.Value, amount.Value);
		System.Console.WriteLine("Withdrawal successful.");
	}

	public void BlockWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		_walletService.Block(playerId.Value, currency.Value);
		System.Console.WriteLine("Wallet blocked.");
	}

	public void UnblockWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		_walletService.Unblock(playerId.Value, currency.Value);
		System.Console.WriteLine("Wallet unblocked.");
	}

	public void UpdateWalletBalance()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var newBalance = Prompts.PromptAmount("New balance");
		if (newBalance is null)
			return;

		_walletService.UpdateBalance(playerId.Value, currency.Value, newBalance.Value);
		System.Console.WriteLine("Balance updated.");
	}

	public void ApplyFundsStrategy()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var operation = Prompts.PromptFundsOperation();
		if (operation is null)
			return;

		var amount = Prompts.PromptAmount("Amount");
		if (amount is null)
			return;

		_walletService.ApplyFundsOperation(playerId.Value, currency.Value, operation.Value, amount.Value);
		System.Console.WriteLine($"{operation} operation applied.");
	}
}
