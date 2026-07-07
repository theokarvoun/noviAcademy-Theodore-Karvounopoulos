namespace WorldRank
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        public InMemoryPlayerRepository()
        {
            Players = new List<Player>();
        }
        public List<Player> Players { get; set; }
        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }
        public Player FindPlayer(int playerId)
        {
            return Players.FirstOrDefault(p => p.Id == playerId) ?? new Player("Unknown", 0);
        }
        public void DeletePlayer(int playerId)
        {
            Player p = Players.FirstOrDefault(p => p.Id == playerId);
            if (p != null)
            {
                Players.Remove(p);
            }
        }
        public List<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return Players.GroupBy(p => p.Score).ToList();
        }
    }
}