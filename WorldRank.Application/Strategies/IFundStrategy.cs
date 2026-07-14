using WorldRank.Domain.Entities;

namespace WorldRank.Application.Strategies
{
    public interface IFundStrategy
    {
        FundsOperation Operation { get; }
        void Execute(Wallet wallet, decimal amount);
    }
}