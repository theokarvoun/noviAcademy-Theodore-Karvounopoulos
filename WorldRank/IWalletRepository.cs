using System.Collections.Generic;
using WorldRank;

namespace WorldRank
{
    public interface IWalletRepository
    {
        public void Add(Wallet wallet, int playerId);
        public List<IWallet>? GetByPlayer(int playerId);

    }
}