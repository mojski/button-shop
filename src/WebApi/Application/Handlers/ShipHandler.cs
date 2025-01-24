namespace ButtonShop.WebApi.Application.Handlers;

using ButtonShop.WebApi.Application.Commands;
using ButtonShop.WebApi.Application.Events;
using ButtonShop.WebApi.Domain.Interfaces;
using MediatR;

public sealed class ShipHandler : IRequestHandler<Ship>
{
    private readonly IPublisher mediator;
    private readonly IOrderRepository repository;

    public ShipHandler(IOrderRepository repository, IPublisher mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task Handle(Ship request, CancellationToken cancellationToken)
    {
        var order = this.repository.GetOrder(request.OrderId);

        if (order is null)
        {
            await Task.CompletedTask;
            // TODO throw application error
        }

        order!.Ship();

        await this.repository.ShipOrder(request.OrderId, cancellationToken);

        var notification = new OrderShipped()
        {
            Id = order.Id,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
