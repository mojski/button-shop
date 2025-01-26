namespace ButtonShop.Application.Events;

using MediatR;

public sealed record OrderShipped : INotification
{
    public required Guid Id { get; init; }
}
