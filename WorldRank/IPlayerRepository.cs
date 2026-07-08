using System.Collections.Generic;

namespace WorldRank
{
    public interface IPlayerRepository
    {
        public void AddPlayer(IPlayer p);
        public IPlayer? FindPlayer(int playerId);
        public void DeletePlayer(int playerId);
        public List<IGrouping<int, IPlayer>> GroupPlayersByScore();
        public List<IPlayer> GetAll();
        public int GenerateId();
    }   
}