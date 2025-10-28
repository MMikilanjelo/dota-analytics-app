using Common.Abstractions.Behaviours;
using Common.Contracts.Messaging;
using Common.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class DependencyInjection
{
    public static IServiceCollection DecorateModules(this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        // services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));
        //
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        // services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        // services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));
        services.AddSingleton<IClock, SystemClock>();


        return services;
    }
}