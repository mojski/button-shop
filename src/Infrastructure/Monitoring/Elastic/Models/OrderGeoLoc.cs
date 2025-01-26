namespace ButtonShop.Infrastructure.Monitoring.Elastic.Models;

public record class OrderGeoLoc
{
    public string GeoLocation => $"{this.Latitude},{this.Longitude}";
    public string? Id { get; private set; } = Guid.NewGuid().ToString();
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
    public required int Quantity { get; init; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
}
