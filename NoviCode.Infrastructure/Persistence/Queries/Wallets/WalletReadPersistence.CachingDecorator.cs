using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Queries.Wallets
{
    // Read-through cache for the three wallet read views. Keys match the write-side
    // decorators so a write and a read agree on where a wallet lives in the cache.
    public class WalletReadPersistenceCachingDecorator : IWalletReadPersistence
    {
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
        private const string AllWalletsKey = "wallets:all";
        private static string WalletKey(Guid id) => $"wallet:{id}";
        private static string PlayerWalletsKey(Guid playerId) => $"wallets:player:{playerId}";

        private readonly IWalletReadPersistence _inner;
        private readonly ICache _cache;

        public WalletReadPersistenceCachingDecorator(IWalletReadPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<Wallet?> GetById(Guid id)
        {
            if (_cache.TryGet(WalletKey(id), out Wallet? cached) && cached is not null)
                return cached;

            var wallet = await _inner.GetById(id);
            if (wallet is not null)
                _cache.Set(WalletKey(id), wallet, Ttl);
            return wallet;
        }

        public async Task<IReadOnlyList<Wallet>> GetByPlayer(Guid playerId)
        {
            if (_cache.TryGet(PlayerWalletsKey(playerId), out IReadOnlyList<Wallet>? cached) && cached is not null)
                return cached;

            var wallets = await _inner.GetByPlayer(playerId);
            _cache.Set(PlayerWalletsKey(playerId), wallets, Ttl);
            return wallets;
        }

        public async Task<IReadOnlyList<Wallet>> GetAll()
        {
            if (_cache.TryGet(AllWalletsKey, out IReadOnlyList<Wallet>? cached) && cached is not null)
                return cached;

            var wallets = await _inner.GetAll();
            _cache.Set(AllWalletsKey, wallets, Ttl);
            return wallets;
        }
    }
}
