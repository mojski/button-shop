namespace ButtonShop.Domain.Interfaces;

using ButtonShop.Domain.Entities;

public interface IOrderRepository
{
    public Order? GetOrder(Guid id);
    public Task SaveOrder(Order order, CancellationToken cancellationToken = default);
    Task ShipOrder(Guid orderId, CancellationToken cancellationToken = default);
}
