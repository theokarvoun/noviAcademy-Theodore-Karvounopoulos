using MediatR;

namespace NoviCode.Queries.Wallets
{
    public record GetWalletByIdQuery(Guid Id) : IRequest<Wallet?>;
}
