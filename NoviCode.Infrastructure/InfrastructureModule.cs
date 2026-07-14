using Autofac;
using NoviCode.Infrastructure;
using NoviCode.Persistence.Commands.Players;
using NoviCode.Persistence.Commands.Wallets;
using NoviCode.Persistence.Queries.Players;
using NoviCode.Persistence.Queries.Wallets;

namespace NoviCode
{
    // Each write/read port is registered as its concrete persistence class, then wrapped by a
    // caching decorator. The handlers depend only on the port, so caching stays invisible to them.
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // --- Players: create (command) ---
            builder.RegisterType<CreatePlayerPersistence>().As<ICreatePlayerPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator(typeof(CreatePlayersPersistenceCachingDecorator), typeof(ICreatePlayerPersistence));

            // --- Players: reads (queries) ---
            builder.RegisterType<PlayerReadPersistence>().As<IPlayerReadPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator(typeof(PlayerReadPersistenceCachingDecorator), typeof(IPlayerReadPersistence));

            // --- Wallets: create (command) ---
            builder.RegisterType<CreateWalletPersistence>().As<ICreateWalletPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator(typeof(CreateWalletPersistenceCachingDecorator), typeof(ICreateWalletPersistence));

            // --- Wallets: mutations — deposit / block / apply funds (commands) ---
            builder.RegisterType<WalletMutationPersistence>().As<IWalletMutationPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator(typeof(WalletMutationPersistenceCachingDecorator), typeof(IWalletMutationPersistence));

            // --- Wallets: reads (queries) ---
            builder.RegisterType<WalletReadPersistence>().As<IWalletReadPersistence>().InstancePerLifetimeScope();
            builder.RegisterDecorator(typeof(WalletReadPersistenceCachingDecorator), typeof(IWalletReadPersistence));
        }
    }
}
