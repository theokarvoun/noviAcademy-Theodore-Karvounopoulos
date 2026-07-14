using MediatR;

namespace NoviCode.Commands.Wallets
{
    public record CreateWalletCommand(Guid PlayerId, Currency Currency) : IRequest<Guid>;
}
