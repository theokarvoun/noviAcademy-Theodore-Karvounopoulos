using NLog;
using WorldRank.Interfaces;

namespace WorldRank
{
    public class Player : IPlayer
    {
        public int Id { get; }
        public string Name { get; private set; }
        public int Score { get; private set; }
        protected Dictionary<Currency, IWallet> Wallets { get; set; }
        private Logger logger = LogManager.GetCurrentClassLogger();
        Dictionary<Currency, IWallet> IPlayer.Wallets { get => Wallets; set => Wallets = value; }

        public List<IWallet> GetWallets()
        {
            return Wallets.Values.ToList();
        }
        public Player(string name, int Id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }
            this.Id = Id;
            this.Name = name;
            Score = 0;
            Wallets = new Dictionary<Currency, IWallet>();
        }
        public void AddScore(int points)
        {
            if (points < 0)
            {
                throw new ArgumentException("Points cannot be negative.", nameof(points));
            }
            Score += points;
            //logger.Info($"Added {points} points to player {Name}. New score: {Score}");
        }
        public override string ToString()
        {
            return $"Id: {Id}, Player: {Name}, Score: {Score}";
        }

        public Dictionary<Currency,IWallet>  GetWalletsDictionary()
        {
            return Wallets;
        }
    }
}
