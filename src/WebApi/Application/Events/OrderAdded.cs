namespace ButtonShop.WebApi.Application.Events;

using ButtonShop.WebApi.Domain.Entities;
using MediatR;

public sealed record OrderAdded : INotification
{
    public required Guid Id { get; init; }
    public required Dictionary<ButtonColors, int> Items { get; init; }
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
}
