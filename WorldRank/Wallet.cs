using NLog;
using WorldRank.Exceptions;
using WorldRank.Interfaces;

namespace WorldRank
{
    public class Wallet : IWallet
    {
        public decimal Balance { get; private set; }
        Logger logger = LogManager.GetCurrentClassLogger();
        public Currency CurrencyType { get; private set; }
        public bool IsBlocked { get; private set; }
        public Wallet(Currency currency) {
            this.CurrencyType = currency;
        }
        
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                logger.Error("Attempted to deposit negative amount");
                throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
                
            }
            Balance += amount;
            logger.Info($"Deposited {amount}");
            
        }
        public void Block() => IsBlocked = true;
        public void UnBlock() => IsBlocked = false;
        public void Withdraw(decimal amount)
        {
            if (IsBlocked)
            {
                throw new BlockedException("Cannot withdraw from a blocked wallet.");
            }
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
            }
            if (amount > Balance)
            {
                throw new InsufficientFundsException("Insufficient funds for withdrawal.");
            }
            Balance -= amount;
        }
        public override string ToString()
        {
            return $"Wallet Balance: {Balance}, Currency: {CurrencyType}, Is Blocked: {IsBlocked}";
        }
    }
    public enum Currency
    {
        USD,
        EUR
    }
}