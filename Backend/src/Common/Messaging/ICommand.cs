namespace WebApi.Common.Messaging;

public interface ICommand : IBaseCommand;

public interface ICommand<TResponse> : IBaseCommand;

public interface IBaseCommand;