namespace Common.Contracts.Events;

public abstract record IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccuredOnUtc { get; private set; } = DateTime.UtcNow;
}

public interface IIntegrationEvent : IEvent
{
    Guid Id { get; }
    DateTime OccuredOnUtc { get; }
}