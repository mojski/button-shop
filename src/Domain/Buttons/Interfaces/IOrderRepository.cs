namespace ButtonShop.Domain.Interfaces;

using ButtonShop.Domain.Entities;

public interface IOrderRepository
{
    Task<Order?> GetOrder(Guid id, CancellationToken cancellationToken = default);
    Task SaveOrder(Order order, CancellationToken cancellationToken = default);
}
