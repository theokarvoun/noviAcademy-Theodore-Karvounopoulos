using Microsoft.Extensions.DependencyInjection;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank.Application.Strategies;

namespace WorldRank.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// All strategies are registered under the same interface. The caller resolves
		// them as a collection and picks the one whose Operation matches - no factory.
		services.AddSingleton<IFundsStrategy, AddFundsStrategy>();
		services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
		services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

		// Application services that drive the use-cases. Consumers depend on the
		// interfaces; the concrete services own the cache-aside / write-through logic.
		services.AddSingleton<IPlayerService, PlayerService>();
		services.AddSingleton<IWalletService, WalletService>();

		return services;
	}
}
