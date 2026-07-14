using System;
using System.Collections.Generic;
using System.Linq;
using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Services
{
    /// <summary>
    /// Applies a funds operation to a wallet by delegating to the matching
    /// <see cref="IFundStrategy"/>. The strategies are injected as a collection
    /// and indexed by their <see cref="FundsOperation"/> - no switch, no factory.
    /// </summary>
    public class FundsService
    {
        private readonly IReadOnlyDictionary<FundsOperation, IFundStrategy> _strategies;

        public FundsService(IEnumerable<IFundStrategy> strategies)
        {
            // Build the operation -> strategy lookup once. ToDictionary throws if two
            // strategies claim the same Operation, surfacing a misconfiguration early.
            _strategies = strategies.ToDictionary(strategy => strategy.Operation);
        }

        public void Execute(
            Wallet wallet,
            decimal amount,
            FundsOperation operation)
        {
            if (!_strategies.TryGetValue(operation, out var strategy))
                throw new ArgumentOutOfRangeException(nameof(operation));

            strategy.Execute(wallet, amount);
        }
    }
}
