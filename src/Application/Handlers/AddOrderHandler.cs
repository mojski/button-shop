namespace ButtonShop.Application.Handlers;

using ButtonShop.Application.Commands;
using ButtonShop.Application.Events;
using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;

public class AddOrderHandler : IRequestHandler<AddOrder>
{
    private readonly IPublisher mediator;
    private readonly IOrderRepository repository;
    private readonly ILogger<AddOrderHandler> logger;

    public AddOrderHandler(IOrderRepository repository, IPublisher mediator, ILogger<AddOrderHandler> logger)
    {
        this.repository = repository;
        this.mediator = mediator;
        this.logger = logger;
    }

    public async Task Handle(AddOrder request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("AddOrder handle for id {id}", request.Id);
        var order = new Order(request.Id, request.CustomerName, request.ShippingAddress);

        foreach (var item in request.Items)
        {
            order.AddItems(item.Key, item.Value);
        }

        await this.repository.SaveOrder(order, cancellationToken);

        var notification = new OrderAdded
        {
            Id = order.Id,
            Items = order.Items,
            Longitude = request.Longitude,
            Latitude = request.Latitude,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }
}
