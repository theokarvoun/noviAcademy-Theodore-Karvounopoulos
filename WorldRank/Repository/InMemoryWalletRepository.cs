using System;
using System.Linq;
using System.Collections.Generic;
using WorldRank.Interfaces;
using NLog;

namespace WorldRank.Repository
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private IPlayerRepository PlayerRepo { get; set; }
        private Logger logger = LogManager.GetCurrentClassLogger();
        public InMemoryWalletRepository(IPlayerRepository playerRepo)
        {
            PlayerRepo = playerRepo;
        }
        public void Add(Wallet wallet, int playerId)
        {
            IPlayer? p = PlayerRepo.FindPlayer(playerId);
            if (p == null)
            {
                logger.Warn("Player does not exist");
                return;
            }
            if (p.GetWalletsDictionary().ContainsKey(wallet.CurrencyType))
            {
                logger.Warn($"Wallet for currency {wallet.CurrencyType} already exists for player {p.Name}. Skipping addition.");
                return;
            }
            p?.GetWalletsDictionary()[wallet.CurrencyType] = wallet;
            logger.Info($"Added wallet {wallet} for player {p?.Name}");
        }
        public List<IWallet>? GetByPlayer(int playerId)
        {
            return PlayerRepo.FindPlayer(playerId)?.GetWallets();
        }
        
    }
}