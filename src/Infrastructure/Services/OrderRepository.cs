namespace ButtonShop.Infrastructure.Services;

using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;

internal class OrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> orders = [];

    public Order? GetOrder(Guid id)
    {
        this.orders.TryGetValue(id, out var order);

        return order;
    }

    public Task SaveOrder(Order order, CancellationToken cancellationToken = default)
    {
        this.orders[order.Id] = order;

        return Task.CompletedTask;
    }

    public Task ShipOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        if (this.orders.TryGetValue(orderId, out var order) is true)
        {
            order!.Ship();
        }

        return Task.CompletedTask;
    }
}
