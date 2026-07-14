using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Interfaces;
using WorldRank.Infrastructure.Caching;
using WorldRank.Infrastructure.Persistence;
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

		services.AddCaching();

		return services;
	}

	/// <summary>
	/// Registers the in-memory cache and the ICache port implementation. The Application
	/// services depend only on ICache; the IMemoryCache-backed MemoryCacheStore is the
	/// swappable Infrastructure detail (Redis would replace it here).
	/// </summary>
	private static IServiceCollection AddCaching(this IServiceCollection services)
	{
		services.AddMemoryCache();
		services.AddSingleton<ICache, MemoryCacheStore>();

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

		services.AddCaching();

		// Day 5 helper: raw-SQL batch upsert via Dapper, sharing the same connection string.
		services.AddSingleton(new WalletBulkUpsert(connectionString));

		return services;
	}
}
