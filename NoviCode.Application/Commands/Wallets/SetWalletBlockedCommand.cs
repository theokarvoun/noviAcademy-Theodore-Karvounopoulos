using MediatR;

namespace NoviCode.Commands.Wallets
{
    public record SetWalletBlockedCommand(Guid WalletId, bool Blocked) : IRequest<Wallet?>;
}
