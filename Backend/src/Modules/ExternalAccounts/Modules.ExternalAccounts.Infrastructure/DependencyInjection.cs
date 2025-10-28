using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ExternalAccounts.Application.Common.Contracts.Clients;
using Modules.ExternalAccounts.Application.Common.Contracts.Services;
using Modules.ExternalAccounts.Domain;
using Modules.ExternalAccounts.Domain.Aggregates;
using Modules.ExternalAccounts.Infrastructure.Data;
using Modules.ExternalAccounts.Infrastructure.Data.Repositories;
using Modules.ExternalAccounts.Infrastructure.Extensions;
using Modules.ExternalAccounts.Infrastructure.Services;
using Modules.ExternalAccounts.Infrastructure.Services.Clients;
using Modules.ExternalAccounts.Infrastructure.Services.Clients.Steam;
using SharedKernel.Contracts;
using StackExchange.Redis;

namespace Modules.ExternalAccounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddExternalAccountsModuleInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddDatabase(configuration)
            .AddClients()
            .AddRepositories();

        services.AddScoped<IUserContextService, UserContextService>();

        services.AddScoped<ILinkTokenService, RedisLinkTokenService>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IExternalAccountLinkRepository, ExternalAccountLinkRepository>();
        services.AddScoped<IExternalAccountsUnitOfWork, ExternalAccountsUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services
            .AddHttpClient<ISteamClient, SteamClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(ExternalAccountEndpoints.Steam.OpenIdUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            });

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ExternalAccountsDbContext>(options =>
            options.UseNpgsql(connectionString, b =>
                b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.ExternalAccounts))
        );

        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect("localhost:6379"));

        return services;
    }
}