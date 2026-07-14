using Microsoft.EntityFrameworkCore;
using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
    public class WalletMutationPersistence : IWalletMutationPersistence
    {
        private readonly AppDbContext _appDbContext;

        public WalletMutationPersistence(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Tracked (no AsNoTracking): the returned entity is mutated by the handler and then saved.
        public Task<Wallet?> GetForUpdate(Guid id) =>
            _appDbContext.Wallets.FirstOrDefaultAsync(w => w.Id == id);

        public Task Save(Wallet wallet) => _appDbContext.SaveChangesAsync();
    }
}
