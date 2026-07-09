using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using WorldRank.Application.Strategies;

namespace WorldRank.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // All strategies are registered under the same interface. The caller resolves
            // them as a collection and picks the one whose Operation matches - no factory.
            services.AddSingleton<IFundStrategy, AddFundsStrategy>();
            services.AddSingleton<IFundStrategy, SubtractFundsStrategy>();
            services.AddSingleton<IFundStrategy, ForceSubtractFundsStrategy>();

            // Application services that drive the menu use-cases.
            //services.AddSingleton<PlayerService>();
           // services.AddSingleton<WalletService>();

            return services;
        }
    }
}
