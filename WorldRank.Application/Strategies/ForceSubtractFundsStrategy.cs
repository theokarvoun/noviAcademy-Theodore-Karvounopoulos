using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Strategies
{
    public class ForceSubtractFundsStrategy : IFundStrategy
    {
        public FundsOperation Operation => FundsOperation.ForceSubtract;
        public void Execute(Wallet wallet, decimal amount)
        {
            wallet.ForceSubtractFunds(amount);
        }
    }
}
