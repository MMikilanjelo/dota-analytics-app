using SharedKernel.Contracts;
using SharedKernel.Contracts.Messaging;

namespace SharedKernel.Abstractions;

public class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}