using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application.Interfaces;

namespace WorldRank.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();
            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
            return services;
        }
    }
}
