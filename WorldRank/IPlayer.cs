namespace WorldRank
{
    public interface IPlayer
    {
        public int Id { get; }
        public string Name { get; }
        public int Score { get; }
        protected Dictionary<Currency,IWallet> Wallets { get; set; }
        public void AddScore(int points);
    }
}