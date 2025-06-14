namespace ButtonShop.Application.Events;

using ButtonShop.Domain.Entities;

public sealed record OrderAdded : INotification
{
    public required Guid Id { get; init; }
    public required IReadOnlyDictionary<ButtonColors, int> Items { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}
