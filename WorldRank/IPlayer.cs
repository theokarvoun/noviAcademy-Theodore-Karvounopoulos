namespace WorldRank
{
    public interface IPlayer
    {
        public int Id { get; }
        public string Name { get; protected set; }
        public int Score { get; protected set; }
        public Dictionary<Wallet.Currency,Wallet> { get; protected set; }
    }
}