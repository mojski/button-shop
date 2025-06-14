namespace ButtonShop.Application.Handlers;

using ButtonShop.Application.Commands;
using ButtonShop.Application.Events;
using ButtonShop.Application.Exceptions;
using ButtonShop.Domain.Interfaces;

public sealed class ShipHandler : IRequestHandler<Ship>
{
    private readonly IPublisher mediator;
    private readonly IOrderRepository repository;
    private readonly ILogger<ShipHandler> logger;

    public ShipHandler(IOrderRepository repository, IPublisher mediator, ILogger<ShipHandler> logger)
    {
        this.repository = repository;
        this.mediator = mediator;
        this.logger = logger;
    }

    public async Task Handle(Ship request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Ship handle for id {id}", request.OrderId);

        var order = await this.repository.GetOrder(request.OrderId);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        if (order.Status is Domain.Entities.OrderStatuses.Shipped)
        {
            return;
        }

        order!.Ship();

        await this.repository.SaveOrder(order, cancellationToken);

        var notification = new OrderShipped()
        {
            Id = order.Id,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
