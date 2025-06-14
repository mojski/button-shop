namespace ButtonShop.Infrastructure.Monitoring.Elastic.Models;

internal record class OrderGeoLoc
{
    public object GeoLocation => new
    {
        lat = this.Latitude, lon = this.Longitude
    };

    public string? Id { get; private set; } = Guid.NewGuid().ToString();
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public required int Quantity { get; init; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
}
