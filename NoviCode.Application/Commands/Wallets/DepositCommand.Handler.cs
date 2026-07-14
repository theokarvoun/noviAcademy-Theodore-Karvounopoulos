using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Commands.Wallets
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Wallet?>
    {
        private readonly IWalletMutationPersistence _persistence;

        public DepositCommandHandler(IWalletMutationPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<Wallet?> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _persistence.GetForUpdate(request.WalletId);
            if (wallet is null)
                return null;

            wallet.Deposit(request.Amount); // may throw WalletBlockedException / InvalidAmountException
            await _persistence.Save(wallet);
            return wallet;
        }
    }
}
