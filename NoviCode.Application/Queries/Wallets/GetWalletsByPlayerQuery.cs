using MediatR;

namespace NoviCode.Queries.Wallets
{
    public record GetWalletsByPlayerQuery(Guid PlayerId) : IRequest<IReadOnlyList<Wallet>>;
}
