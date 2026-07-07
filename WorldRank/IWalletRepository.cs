using System.Collections.Generic;
using WorldRank;

namespace WorldRank
{
    public interface IWalletRepository
    {
        public InMemoryPlayerRepository PlayerRepo { get; protected set; }
        public void Add(Wallet wallet, int playerId);
        public List<Wallet> GetByPlayer(int playerId);

    }
}