using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Commands.Wallets
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Guid>
    {
        private readonly ICreateWalletPersistence _persistence;

        public CreateWalletCommandHandler(ICreateWalletPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = new Wallet(request.PlayerId, request.Currency);
            await _persistence.Persist(wallet);
            return wallet.Id;
        }
    }
}
