using NLog;
using WorldRank.Interfaces;

namespace WorldRank.Repository
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private int _id = 0;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public InMemoryPlayerRepository()
        {
            Players = new List<IPlayer>();
        }
        public int GenerateId()
        {
            return _id++;
        }
        private List<IPlayer> Players { get; set; }
        public void AddPlayer(IPlayer p)
        {
            Players.Add(p);
            logger.Info($"Player added: {p}");
        }
        public IPlayer? FindPlayer(int playerId)
        {
            return Players.FirstOrDefault(p => p.Id == playerId);
        }
        public List<IPlayer> GetAll()
        {
            return Players;
        }
        public void DeletePlayer(int playerId)
        {
            IPlayer? p = Players.FirstOrDefault(p => p.Id == playerId);
            if (p != null)
            {
                Players.Remove(p);
            }
        }
        public List<IGrouping<int, IPlayer>> GroupPlayersByScore()
        {
            return Players.GroupBy(p => p.Score).ToList() ?? new List<IGrouping<int,IPlayer>>();
        }
    }
}