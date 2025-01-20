namespace ButtonShop.WebApi.Infrastructure.Services;

using ButtonShop.WebApi.Application.Events;
using ButtonShop.WebApi.Domain.Entities;
using ButtonShop.WebApi.Domain.Interfaces;
using MediatR;

public class OrderRepository : IOrderRepository
{
    private readonly IPublisher mediator;
    private readonly Dictionary<Guid, Order> orders = [];

    public OrderRepository(IPublisher mediator)
    {
        this.mediator = mediator;
    }

    public Order? GetOrder(Guid id)
    {
        this.orders.TryGetValue(id, out var order);

        return order;
    }

    public async Task SaveOrder(Order order, CancellationToken cancellationToken = default)
    {
        this.orders[order.Id] = order;

        var notification = new OrderAdded
        {
            Id = order.Id,
            Items = order.Items,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }

    public async Task ShipOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        if (this.orders.TryGetValue(orderId, out var order) is false)
        {
            return;
        }

        order!.Ship();

        var notification = new OrderAdded
        {
            Id = order.Id,
            Items = order.Items,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
