namespace WorldRank
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private int _id = 0;
        public InMemoryPlayerRepository()
        {
            Players = new List<Player>();
        }
        public int GenerateId()
        {
            return _id++;
        }
        private List<Player> Players { get; set; }
        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }
        public Player? FindPlayer(int playerId)
        {
            return Players.FirstOrDefault(p => p.Id == playerId);
        }
        public List<Player> GetAll()
        {
            return Players;
        }
        public void DeletePlayer(int playerId)
        {
            Player? p = Players.FirstOrDefault(p => p.Id == playerId);
            if (p != null)
            {
                Players.Remove(p);
            }
        }
        public List<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return Players.GroupBy(p => p.Score).ToList() ?? new List<IGrouping<int,Player>>();
        }
    }
}