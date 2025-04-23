namespace ButtonShop.Infrastructure.Services;

using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;

internal class OrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> orders = [];
    private readonly ILogger<OrderRepository> logger;

    public OrderRepository(ILogger<OrderRepository> logger)
    {
        this.logger = logger;
    }

    public Order? GetOrder(Guid id)
    {
        this.logger.LogInformation("Getting order with id {id}", id);
        this.orders.TryGetValue(id, out var order);

        return order;
    }

    public Task SaveOrder(Order order, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Saving order with id {id}", order.Id);
        this.orders[order.Id] = order;

        return Task.CompletedTask;
    }

    public Task ShipOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Shipping order with id {orderId}", orderId);
        if (this.orders.TryGetValue(orderId, out var order) is true)
        {
            order!.Ship();
        }

        return Task.CompletedTask;
    }
}
