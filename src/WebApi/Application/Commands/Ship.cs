namespace ButtonShop.WebApi.Application.Commands;

using System.Text.Json.Serialization;
using MediatR;

public sealed record class Ship : IRequest
{
    [JsonPropertyName("orderId")]
    public required Guid OrderId { get; init; }
}
