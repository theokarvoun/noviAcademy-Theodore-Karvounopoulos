using Microsoft.EntityFrameworkCore;
using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Queries.Wallets
{
    public class WalletReadPersistence : IWalletReadPersistence
    {
        private readonly AppDbContext _db;

        public WalletReadPersistence(AppDbContext db) => _db = db;

        public Task<Wallet?> GetById(Guid id) =>
            _db.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);

        public async Task<IReadOnlyList<Wallet>> GetByPlayer(Guid playerId) =>
            await _db.Wallets.AsNoTracking().Where(w => w.PlayerId == playerId).ToListAsync();

        public async Task<IReadOnlyList<Wallet>> GetAll() =>
            await _db.Wallets.AsNoTracking().ToListAsync();
    }
}
