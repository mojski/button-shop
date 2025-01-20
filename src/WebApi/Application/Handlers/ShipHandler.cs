namespace ButtonShop.WebApi.Application.Handlers;

using ButtonShop.WebApi.Application.Commands;
using ButtonShop.WebApi.Domain.Interfaces;
using MediatR;

public sealed class ShipHandler : IRequestHandler<Ship>
{
    private readonly IOrderRepository repository;

    public ShipHandler(IOrderRepository repository)
    {
        this.repository = repository;
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
    }
}
