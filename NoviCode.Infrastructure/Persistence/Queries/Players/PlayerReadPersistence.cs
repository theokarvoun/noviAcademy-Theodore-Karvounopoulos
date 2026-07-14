using Microsoft.EntityFrameworkCore;
using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Queries.Players
{
    // The real read side: talks to EF Core. AsNoTracking — these entities are never mutated.
    public class PlayerReadPersistence : IPlayerReadPersistence
    {
        private readonly AppDbContext _db;

        public PlayerReadPersistence(AppDbContext db) => _db = db;

        public Task<Player?> GetById(Guid id) =>
            _db.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IReadOnlyList<Player>> GetAll() =>
            await _db.Players.AsNoTracking().ToListAsync();
    }
}
