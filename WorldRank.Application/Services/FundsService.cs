using System;
using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Services
{
    public class FundsService
    {
        public void Execute(
            Wallet wallet,
            decimal amount,
            FundsOperation operation)
        {
            switch (operation)
            {
                case FundsOperation.Add:
                    wallet.Deposit(amount);
                    break;

                case FundsOperation.Subtract:
                    wallet.Withdraw(amount);
                    break;

                case FundsOperation.ForceSubtract:
                    wallet.ForceSubtractFunds(amount);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
    }
}
