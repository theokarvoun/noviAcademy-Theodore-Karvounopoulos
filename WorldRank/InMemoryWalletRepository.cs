using System;
using System.Linq;
using System.Collections.Generic;

namespace WorldRank
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private IPlayerRepository PlayerRepo { get; set; }
        public InMemoryWalletRepository(IPlayerRepository playerRepo)
        {
            PlayerRepo = playerRepo;
        }
        public void Add(Wallet wallet, int playerId)
        {
            IPlayer? p = PlayerRepo.FindPlayer(playerId);
            if (p != null && p.GetWalletsDictionary().ContainsKey(wallet.CurrencyType))
            {
                Console.WriteLine($"Wallet for currency {wallet.CurrencyType} already exists for player {p.Name}. Skipping addition.");
                return;
            }
            p?.GetWalletsDictionary()[wallet.CurrencyType] = wallet;
        }
        public List<IWallet>? GetByPlayer(int playerId)
        {
            return PlayerRepo.FindPlayer(playerId)?.GetWallets();
        }
        
    }
}