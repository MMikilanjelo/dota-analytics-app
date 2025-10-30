namespace SharedKernel;

public class InboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = "";
    public string Payload { get; init; } = "";
    public DateTime ReceivedOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}