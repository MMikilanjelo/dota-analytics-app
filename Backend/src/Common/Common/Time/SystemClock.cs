namespace Common.Time;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
    
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow.Date);
}