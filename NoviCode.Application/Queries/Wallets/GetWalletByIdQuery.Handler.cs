using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Queries.Wallets
{
    public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, Wallet?>
    {
        private readonly IWalletReadPersistence _persistence;

        public GetWalletByIdQueryHandler(IWalletReadPersistence persistence)
        {
            _persistence = persistence;
        }

        public Task<Wallet?> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
            => _persistence.GetById(request.Id);
    }
}
