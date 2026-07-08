using System.Collections.Generic;

namespace WorldRank.Interfaces
{
    public interface IWalletRepository
    {
        public void Add(Wallet wallet, int playerId);
        public List<IWallet>? GetByPlayer(int playerId);

    }
}