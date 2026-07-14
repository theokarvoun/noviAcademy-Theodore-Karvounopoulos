using MediatR;
using NoviCode.Infrastructure;

namespace NoviCode.Queries.Wallets
{
    public class GetWalletsByPlayerQueryHandler : IRequestHandler<GetWalletsByPlayerQuery, IReadOnlyList<Wallet>>
    {
        private readonly IWalletReadPersistence _persistence;

        public GetWalletsByPlayerQueryHandler(IWalletReadPersistence persistence)
        {
            _persistence = persistence;
        }

        public Task<IReadOnlyList<Wallet>> Handle(GetWalletsByPlayerQuery request, CancellationToken cancellationToken)
            => _persistence.GetByPlayer(request.PlayerId);
    }
}
