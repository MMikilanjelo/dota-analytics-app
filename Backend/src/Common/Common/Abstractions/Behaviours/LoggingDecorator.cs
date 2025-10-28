using Common.Contracts.Messaging;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Common.Abstractions.Behaviours;

internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            Result<TResponse> operationResult = await innerHandler.HandleAsync(command, cancellationToken);

            if (operationResult.IsSuccess)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                using (LogContext.PushProperty("Error", operationResult.DomainError, true))
                {
                    logger.LogError("Completed command {Command} with {Error}", commandName, operationResult.DomainError);
                }
            }

            return operationResult;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandBaseHandler<TCommand>> logger) : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            Result operationResult = await innerHandler.HandleAsync(command, cancellationToken);

            if (operationResult.IsSuccess)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                using (LogContext.PushProperty("Error", operationResult.DomainError, true))
                {
                    logger.LogError("Completed command {Command} with {Error}", commandName, operationResult.DomainError);
                }
            }

            return operationResult;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger
    ) : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            string queryName = typeof(TQuery).Name;

            logger.LogInformation("Processing query {Query}", queryName);

            Result<TResponse> operationResult = await innerHandler.HandleAsync(query, cancellationToken);

            if (operationResult.IsSuccess)
            {
                logger.LogInformation("Completed query {Query}", queryName);
            }
            else
            {
                using (LogContext.PushProperty("Error", operationResult.DomainError, true))
                {
                    logger.LogError("Completed query {Query} with {Error}", queryName, operationResult.DomainError);
                }
            }

            return operationResult;
        }
    }
}