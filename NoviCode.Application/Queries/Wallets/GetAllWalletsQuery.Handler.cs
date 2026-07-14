using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Queries.Wallets
{
    public class GetAllWalletsQueryHandler : IRequestHandler<GetAllWalletsQuery, IReadOnlyList<Wallet>>
    {
        private readonly IWalletReadPersistence _persistence;

        public GetAllWalletsQueryHandler(IWalletReadPersistence persistence)
        {
            _persistence = persistence;
        }

        public Task<IReadOnlyList<Wallet>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
            => _persistence.GetAll();
    }
}
