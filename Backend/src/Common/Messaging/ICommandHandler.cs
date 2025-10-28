namespace WebApi.Common.Messaging;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<OperationResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<OperationResult<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}