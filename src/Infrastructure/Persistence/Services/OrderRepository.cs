namespace ButtonShop.Infrastructure.Persistence.Services;

using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;
using ButtonShop.Infrastructure.Persistence.Extensions;
using ButtonShop.Infrastructure.Persistence.Models;

internal class OrderRepository : IOrderRepository
{
    private readonly IDocumentStore store;
    private readonly ILogger<OrderRepository> logger;

    public OrderRepository(IDocumentStore store, ILogger<OrderRepository> logger)
    {
        this.logger = logger;
        this.store = store;
    }

    public async Task<Order?> GetOrder(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting order with id {id}", id);

        using var session = store.QuerySession();
        var dbModel =  await session.LoadAsync<OrderDbModel>(id, cancellationToken);

        if (dbModel == null) 
        {
            return null;
        }

        return dbModel.ToDomainModel();
    }

    public async Task SaveOrder(Order order, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Saving order with id {id}", order.Id);

        await using var session = store.LightweightSession();
        var dbModel = order.ToDbModel();

        session.Store([dbModel]);
        
        await session.SaveChangesAsync(cancellationToken);
    }
}
