using System.Collections.Generic;

namespace WorldRank
{
    public interface IPlayerRepository
    {
        public List<Player> Players { get; protected set; }
        public void AddPlayer(Player p);
        public Player FindPlayer(int playerId);
        public void DeletePlayer(int playerId);
        public List<IGrouping<int, Player>> GroupPlayersByScore();
    }
}