using SharedKernel.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;
using Modules.Analytics.Infrastructure.Data;
using Modules.Analytics.Infrastructure.Data.Repositories;
using Modules.Analytics.Infrastructure.Jobs;
using Modules.Analytics.Infrastructure.Listeners;
using Modules.Analytics.Infrastructure.Options;
using Modules.Analytics.Infrastructure.Services.OpenDotaClient;
using Quartz;
using Quartz.Impl.Matchers;

namespace Modules.Analytics.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAnalyticsModuleInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(
            configuration.GetSection(MongoDbOptions.SectionName)
        );

        services.AddDatabase(configuration);

        services.AddScoped<IDotaMatchRepository, DotaMatchRepository>();

        services
            .AddHttpClient<IOpenDotaService, OpenDotaService>((sp, client) => { client.BaseAddress = new Uri("https://api.opendota.com/api/"); })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            });

        return services;
    }

    public static IServiceCollectionQuartzConfigurator AddAnalyticsJobs(this IServiceCollectionQuartzConfigurator q)
    {
        q.AddJob<FetchDotaMatchesJob>(opts =>
        {
            opts.WithIdentity(FetchDotaMatchesJob.Key);
            opts.StoreDurably();
        });
        q.AddJob<ParseDotaMatchReplayJob>(opts =>
        {
            opts.WithIdentity(ParseDotaMatchReplayJob.Key);
            opts.StoreDurably();
        });
        q.AddJobListener<FetchDotaMatchesJobListener>(
            KeyMatcher<JobKey>.KeyEquals(FetchDotaMatchesJob.Key)
        );

        q.AddTrigger(t => t
            .ForJob(FetchDotaMatchesJob.Key)
            .WithIdentity(nameof(FetchDotaMatchesJob))
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromMinutes(2))
                .RepeatForever()));

        return q;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AnalyticsDbContext>(options =>
            options.UseNpgsql(connectionString, b =>
                b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Analytics))
        );

        return services;
    }
}