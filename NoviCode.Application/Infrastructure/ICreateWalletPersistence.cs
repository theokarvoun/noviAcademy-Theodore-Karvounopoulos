namespace NoviCode.Infrastructure
{
    // Write-side port for creating a wallet (mirrors ICreatePlayerPersistence).
    public interface ICreateWalletPersistence
    {
        Task Persist(Wallet wallet);
    }
}
