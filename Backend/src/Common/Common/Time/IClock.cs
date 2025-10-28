namespace Common.Time;

public interface IClock
{
    DateTime UtcNow { get; }
    DateOnly Today { get; }
}