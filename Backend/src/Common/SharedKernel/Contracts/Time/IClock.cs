namespace SharedKernel.Contracts.Time;

public interface IClock
{
    DateTime UtcNow { get; }
    DateOnly Today { get; }
}