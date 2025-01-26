namespace ButtonShop.Application.Commands;

using System.Text.Json.Serialization;
using MediatR;

public sealed record class AddOrder : IRequest
{
    [JsonPropertyName("customerName")]
    public required string CustomerName { get; init; }

    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("items")]
    public Dictionary<string, int> Items { get; init; } = new();

    [JsonPropertyName("latitude")]
    public required string Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public required string Longitude { get; init; }

    [JsonPropertyName("shippingAddress")]
    public required string ShippingAddress { get; init; }
}
