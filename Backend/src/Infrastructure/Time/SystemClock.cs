using SharedKernel.Contracts;
using SharedKernel.Contracts.Time;

namespace Infrastructure.Time;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
    
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow.Date);
}