using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
    // Write-through on create: cache the new wallet under its own key and invalidate the two
    // list views (all wallets + this player's wallets) so the next read rebuilds them.
    public class CreateWalletPersistenceCachingDecorator : ICreateWalletPersistence
    {
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
        private const string AllWalletsKey = "wallets:all";
        private static string WalletKey(Guid id) => $"wallet:{id}";
        private static string PlayerWalletsKey(Guid playerId) => $"wallets:player:{playerId}";

        private readonly ICreateWalletPersistence _inner;
        private readonly ICache _cache;

        public CreateWalletPersistenceCachingDecorator(ICreateWalletPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task Persist(Wallet wallet)
        {
            await _inner.Persist(wallet);

            _cache.Set(WalletKey(wallet.Id), wallet, Ttl);
            _cache.Remove(AllWalletsKey);
            _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        }
    }
}
