using NoviCode.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
    public class CreateWalletPersistence : ICreateWalletPersistence
    {
        private readonly AppDbContext _appDbContext;

        public CreateWalletPersistence(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Persist(Wallet wallet)
        {
            _appDbContext.Add(wallet);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
