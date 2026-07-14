using MediatR;

namespace NoviCode.Queries.Wallets
{
    public record GetAllWalletsQuery() : IRequest<IReadOnlyList<Wallet>>;
}
