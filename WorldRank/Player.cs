namespace WorldRank
{
    public class Player : IPlayer
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Score { get; set; }
        public Dictionary<Wallet.Currency, Wallet> Wallets { get; set; }
        public Player(string name, int Id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }
            this.Id = Id;
            this.Name = name;
            Score = 0;
            Wallets = new Dictionary<Wallet.Currency, Wallet>();
        }
        public void AddScore(int points)
        {
            if (points < 0)
            {
                throw new ArgumentException("Points cannot be negative.", nameof(points));
            }
            Score += points;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Player: {Name}, Score: {Score}";
        }
    }
}
