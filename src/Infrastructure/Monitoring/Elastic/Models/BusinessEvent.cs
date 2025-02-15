namespace ButtonShop.Infrastructure.Monitoring.Elastic.Models;

internal record class BusinessEvent
{
    public string? Id { get; private set; } = Guid.NewGuid().ToString();
    public required string Level { get; init; }
    public required string Message { get; init; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
}
