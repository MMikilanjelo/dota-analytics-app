using SharedKernel.Contracts;
using SharedKernel.Contracts.Messaging;
using SharedKernel.Contracts.Time;
using Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddScoped<IEventPublisher, EventPublisher>();
        return services;
    }
}