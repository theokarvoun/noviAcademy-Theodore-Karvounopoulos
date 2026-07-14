namespace NoviCode.Infrastructure
{
    // Read-side persistence port for wallets (the Query side of CQRS).
    public interface IWalletReadPersistence
    {
        Task<Wallet?> GetById(Guid id);
        Task<IReadOnlyList<Wallet>> GetByPlayer(Guid playerId);
        Task<IReadOnlyList<Wallet>> GetAll();
    }
}
