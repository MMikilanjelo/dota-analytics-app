﻿namespace WebApi.Common.Messaging;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<OperationResult<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}