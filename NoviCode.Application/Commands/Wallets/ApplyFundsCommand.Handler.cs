using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Commands.Wallets
{
    public class ApplyFundsCommandHandler : IRequestHandler<ApplyFundsCommand, Wallet?>
    {
        private readonly IWalletMutationPersistence _persistence;

        public ApplyFundsCommandHandler(IWalletMutationPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<Wallet?> Handle(ApplyFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _persistence.GetForUpdate(request.WalletId);
            if (wallet is null)
                return null;

            // Map the requested operation (data) to a strategy (behaviour) — Strategy pattern,
            // no balance mutation logic here.
            IFundsStrategy strategy = request.Operation switch
            {
                FundsOperation.Add => new AddFundsStrategy(),
                FundsOperation.Subtract => new SubtractFundsStrategy(),
                FundsOperation.ForceSubtract => new ForceSubtractFundsStrategy(),
                _ => throw new ArgumentOutOfRangeException(nameof(request.Operation), request.Operation, "Unknown funds operation.")
            };

            strategy.Execute(wallet, request.Amount);
            await _persistence.Save(wallet);
            return wallet;
        }
    }
}
