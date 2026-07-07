using System.Linq;
using System.Collections.Generic;

namespace WorldRank
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        public InMemoryPlayerRepository PlayerRepo { get; set; } 
        public InMemoryWalletRepository(InMemoryPlayerRepository playerRepo)
        {
            PlayerRepo = playerRepo;
        }
        public void Add(Wallet wallet, int playerId)
        {
            Player p = PlayerRepo.Players.FirstOrDefault(p => p.Id == playerId);
            if (p.Wallets.ContainsKey(wallet.CurrencyType))
            {
                Console.WriteLine($"Wallet for currency {wallet.CurrencyType} already exists for player {p.Name}. Skipping addition.");
                return;
            }
            p.Wallets[wallet.CurrencyType] = wallet;
        }
        public List<Wallet> GetByPlayer(int playerId)
        {
            return PlayerRepo.Players.FirstOrDefault(p => p.Id == playerId)?.Wallets.Values.ToList();
        }
        
    }
}