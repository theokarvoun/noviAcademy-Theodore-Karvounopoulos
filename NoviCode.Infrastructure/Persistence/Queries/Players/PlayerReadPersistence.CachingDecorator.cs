using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Queries.Players
{
    // Read-through cache (cache-aside): serve from ICache on a hit; on a miss, load via the
    // inner persistence and populate the cache. This is the caching that used to live inline
    // in PlayerService.GetByIdAsync / GetAllAsync, now a decorator over the read port.
    public class PlayerReadPersistenceCachingDecorator : IPlayerReadPersistence
    {
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);
        private const string AllPlayersKey = "players:all";
        private static string PlayerKey(Guid id) => $"player:{id}";

        private readonly IPlayerReadPersistence _inner;
        private readonly ICache _cache;

        public PlayerReadPersistenceCachingDecorator(IPlayerReadPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<Player?> GetById(Guid id)
        {
            if (_cache.TryGet(PlayerKey(id), out Player? cached) && cached is not null)
                return cached;

            var player = await _inner.GetById(id);
            if (player is not null)
                _cache.Set(PlayerKey(id), player, Ttl);
            return player;
        }

        public async Task<IReadOnlyList<Player>> GetAll()
        {
            if (_cache.TryGet(AllPlayersKey, out IReadOnlyList<Player>? cached) && cached is not null)
                return cached;

            var players = await _inner.GetAll();
            _cache.Set(AllPlayersKey, players, Ttl);
            return players;
        }
    }
}
