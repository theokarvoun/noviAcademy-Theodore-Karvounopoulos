using MediatR;

namespace NoviCode.Commands.Wallets
{
    // The command carries the operation as data (FundsOperation); the handler maps it to the
    // matching IFundsStrategy. Returns the updated wallet, or null if the wallet is not found.
    public record ApplyFundsCommand(Guid WalletId, decimal Amount, FundsOperation Operation) : IRequest<Wallet?>;
}
