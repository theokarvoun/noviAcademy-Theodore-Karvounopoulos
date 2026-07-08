namespace WorldRank.Interfaces
{
    public interface IWallet
    {
        public abstract decimal Balance { get; }
        public abstract Currency CurrencyType { get; }
        public abstract bool IsBlocked { get;}

        public void Block();
        public void Deposit(decimal amount);
        public void UnBlock();
        public void Withdraw(decimal amount);
    }
}