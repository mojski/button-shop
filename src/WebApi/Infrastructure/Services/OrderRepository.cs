namespace ButtonShop.WebApi.Infrastructure.Services;

using ButtonShop.WebApi.Domain.Entities;
using ButtonShop.WebApi.Domain.Interfaces;

public class OrderRepository : IOrderRepository
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

    public async Task ShipOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        if (this.orders.TryGetValue(orderId, out var order) is false)
        {
            return;
        }

        order!.Ship();
    }
}
