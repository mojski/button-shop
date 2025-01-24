namespace ButtonShop.WebApi.Infrastructure.Monitoring.Elastic.Models;

public record class BusinessEvent
{
    public string? Id { get; private set; } = Guid.NewGuid().ToString();
    public required string Level { get; init; }
    public required string Message { get; init; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
}
