using System.Text;
using Common.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Modules.Users.Application.UseCases.Contracts.Services;
using Modules.Users.Domain;
using Modules.Users.Domain.Aggregates.RefreshTokenAggregate;
using Modules.Users.Domain.Aggregates.UserAggregate;
using Modules.Users.Infrastructure.Data;
using Modules.Users.Infrastructure.Data.DataLoaders;
using Modules.Users.Infrastructure.Data.Repositories;
using Modules.Users.Infrastructure.Jobs;
using Modules.Users.Infrastructure.Services;
using Quartz;
using Quartz.Impl.Matchers;
using IUserByIdDataLoader = Modules.Users.Application.UseCases.Contracts.DataLoaders.IUserByIdDataLoader;
using IUserByEmailDataLoader = Modules.Users.Application.UseCases.Contracts.DataLoaders.IUserByEmailDataLoader;
using IRefreshTokenByTokenDataLoader = Modules.Users.Application.UseCases.Contracts.DataLoaders.IRefreshTokenByTokenDataLoader;

namespace Modules.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModuleInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddScoped<IUserByEmailDataLoader>(sp => sp.GetRequiredService<UserByEmailDataLoader>())
            .AddScoped<IUserByIdDataLoader>(sp => sp.GetRequiredService<UserByIdDataLoader>())
            .AddScoped<IRefreshTokenByTokenDataLoader>(sp => sp.GetRequiredService<RefreshTokenByTokenDataLoader>())
            .AddDatabase(configuration)
            .AddDataLoaders()
            .AddRepositories()
            .AddScoped<IUserByEmailDataLoader>(
                sp => sp.GetRequiredService<UserByEmailDataLoader>()
            )
            .AddScoped<IUserByIdDataLoader>(
                sp => sp.GetRequiredService<UserByIdDataLoader>()
            )
            .AddScoped<IRefreshTokenByTokenDataLoader>(
                sp => sp.GetRequiredService<RefreshTokenByTokenDataLoader>()
            )
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
                    )
                };
            });

        services.AddQuartz(q =>
        {
            q.AddJob<ProcessUsersOutboxMessagesJob>(opts => { opts.WithIdentity(ProcessUsersOutboxMessagesJob.Key); });

            q.AddTrigger(t => t
                .ForJob(ProcessUsersOutboxMessagesJob.Key)
                .WithIdentity(nameof(ProcessUsersOutboxMessagesJob))
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromSeconds(3))
                    .RepeatForever()));
        });


        services.AddHttpContextAccessor();
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
        services.AddSingleton<IIdentityService, IdentityService>();

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddDataLoaders(this IServiceCollection services)
    {
        services.AddUserDataLoader();

        services.AddScoped<IUserByEmailDataLoader>(
            sp => sp.GetRequiredService<UserByEmailDataLoader>()
        );

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<UsersDbContext>(options =>
            options.UseNpgsql(connectionString, b =>
                b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
        );

        return services;
    }
}