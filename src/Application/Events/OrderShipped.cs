namespace ButtonShop.Application.Events;

public sealed record OrderShipped : INotification
{
    public required Guid Id { get; init; }
}
