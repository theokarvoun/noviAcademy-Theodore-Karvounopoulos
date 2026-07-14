using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
    // Write-through on mutation: after Save, refresh the wallet's own cache entry and drop the
    // list views. GetForUpdate is NOT cached — a mutation needs a tracked entity from the store.
    public class WalletMutationPersistenceCachingDecorator : IWalletMutationPersistence
    {
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
        private const string AllWalletsKey = "wallets:all";
        private static string WalletKey(Guid id) => $"wallet:{id}";
        private static string PlayerWalletsKey(Guid playerId) => $"wallets:player:{playerId}";

        private readonly IWalletMutationPersistence _inner;
        private readonly ICache _cache;

        public WalletMutationPersistenceCachingDecorator(IWalletMutationPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public Task<Wallet?> GetForUpdate(Guid id) => _inner.GetForUpdate(id);

        public async Task Save(Wallet wallet)
        {
            await _inner.Save(wallet);

            _cache.Set(WalletKey(wallet.Id), wallet, Ttl);
            _cache.Remove(AllWalletsKey);
            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        }
    }
}
