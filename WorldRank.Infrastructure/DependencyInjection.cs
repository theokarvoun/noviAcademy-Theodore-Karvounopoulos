using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Interfaces;
using WorldRank.Infrastructure.Persistence.Context;
using WorldRank.Infrastructure.Repositories;

namespace WorldRank.Infrastructure;

public static class DependencyInjection
{
	/// <summary>
	/// Registers the in-memory repositories. State lives for the whole app, so Singleton.
	/// </summary>
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
		services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();

		return services;
	}

	/// <summary>
	/// Registers the Entity Framework Core (SQL Server) backed repositories.
	/// Because the rest of the app is composed as Singletons and the console menu is
	/// single-threaded, the DbContext and repositories are registered as Singletons too
	/// (this avoids a Singleton service capturing a shorter-lived Scoped DbContext).
	/// The consuming code depends only on IPlayerRepository / IWalletRepository, so
	/// swapping AddInfrastructure() for this method needs no other code changes.
	/// </summary>
	public static IServiceCollection AddInfrastructureDatabase(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<WorldRankDBContext>(
			options => options.UseSqlServer(connectionString),
			contextLifetime: ServiceLifetime.Singleton,
			optionsLifetime: ServiceLifetime.Singleton);

		services.AddSingleton<IPlayerRepository, DBPlayerRepository>();
		services.AddSingleton<IWalletRepository, DBWalletRepository>();

		return services;
	}
}
