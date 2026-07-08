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
                // Let caller handle logging of exceptions with full context
                throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
                
            }
            Balance += amount;
            logger.Info($"Deposited {amount}");
            
        }
        public void Block()
        {
            IsBlocked = true;
            logger.Info($"Wallet {this} blocked.");
        }
        public void UnBlock()
        {
            IsBlocked = false;
            logger.Info($"Wallet {this} unblocked.");
        }
        public void Withdraw(decimal amount)
        {
            if (IsBlocked)
            {
                // Let caller log the exception with stack trace/context
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
            logger.Info($"Withdrawn {amount}");
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