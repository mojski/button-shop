namespace ButtonShop.WebApi.Application.Handlers;

using ButtonShop.WebApi.Application.Commands;
using ButtonShop.WebApi.Application.Events;
using ButtonShop.WebApi.Domain.Entities;
using ButtonShop.WebApi.Domain.Interfaces;
using MediatR;

public class AddOrderHandler : IRequestHandler<AddOrder>
{
    private readonly IPublisher mediator;
    private readonly IOrderRepository repository;

    public AddOrderHandler(IOrderRepository repository, IPublisher mediator)
    {
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task Handle(AddOrder request, CancellationToken cancellationToken)
    {
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
