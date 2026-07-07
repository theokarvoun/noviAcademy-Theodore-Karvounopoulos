namespace WorldRank
{
    public class Wallet
    {
        public decimal Balance { get; private set; }
        public enum Currency
        {
            USD,
            EUR
        }
        public Currency CurrencyType { get; private set; }
        public bool IsBlocked { get; private set; }
        public Wallet(Currency currency)
        {
            this.CurrencyType = currency;
        }
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
            }
            Balance += amount;
        }
        
    }
}