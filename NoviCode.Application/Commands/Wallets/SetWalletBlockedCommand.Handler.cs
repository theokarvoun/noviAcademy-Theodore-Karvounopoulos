using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Commands.Wallets
{
    public class SetWalletBlockedCommandHandler : IRequestHandler<SetWalletBlockedCommand, Wallet?>
    {
        private readonly IWalletMutationPersistence _persistence;

        public SetWalletBlockedCommandHandler(IWalletMutationPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<Wallet?> Handle(SetWalletBlockedCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _persistence.GetForUpdate(request.WalletId);
            if (wallet is null)
                return null;

            if (request.Blocked)
                wallet.Block();
            else
                wallet.Unblock();

            await _persistence.Save(wallet);
            return wallet;
        }
    }
}
