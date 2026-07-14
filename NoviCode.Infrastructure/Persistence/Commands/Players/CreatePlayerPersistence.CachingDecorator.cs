using NoviCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Persistence.Commands.Players
{
    public class CreatePlayerPersistenceCachingDecorator : ICreatePlayerPersistence
    {
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

        private readonly CreatePlayerPersistence _inner;
        private readonly ICache _cache;

        public CreatePlayerPersistenceCachingDecorator(CreatePlayerPersistence inner, ICache cache)
        {
            _inner = inner;
            _cache = cache;
        }
        public async Task Persist(Player player)
        {
            await _inner.Persist(player);
            //cache player
            _cache.Set($"player:{player.Id}", player, Ttl);
        }
    }
}
