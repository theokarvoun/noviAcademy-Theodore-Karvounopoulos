namespace NoviCode.Infrastructure
{
    // Write-side port for operations that mutate an EXISTING wallet (deposit, block, apply
    // funds): load a tracked entity, then persist the change. GetForUpdate returns a tracked
    // entity straight from the store (never the cache) so the mutation is saved correctly.
    public interface IWalletMutationPersistence
    {
        Task<Wallet?> GetForUpdate(Guid id);
        Task Save(Wallet wallet);
    }
}
