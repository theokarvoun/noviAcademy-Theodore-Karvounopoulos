using MediatR;

namespace NoviCode.Commands.Wallets
{
    // Returns the updated wallet, or null if no wallet with that id exists.
    public record DepositCommand(Guid WalletId, decimal Amount) : IRequest<Wallet?>;
}
