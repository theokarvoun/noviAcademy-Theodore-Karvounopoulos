using System.Collections.Generic;

namespace WorldRank
{
    public interface IWalletRepository
    {
        public InMemoryPlayerRepository PlayerRepo { get; protected set; }
        public void Add(Wallet wallet, int playerId);
        public List<Wallet> GetByPlayer(int playerId);

    }
}