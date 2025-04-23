namespace ButtonShop.Application.Commands;

public sealed record class Ship : IRequest
{
    [JsonPropertyName("orderId")]
    public required Guid OrderId { get; init; }
}
