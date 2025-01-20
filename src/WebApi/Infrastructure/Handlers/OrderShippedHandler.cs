namespace ButtonShop.WebApi.Infrastructure.Handlers;

using ButtonShop.WebApi.Application.Events;
using ButtonShop.WebApi.Infrastructure.Metrics;
using MediatR;

public sealed class OrderShippedHandler : INotificationHandler<OrderShipped>
{
    private readonly IMetricsService metricsService;

    public OrderShippedHandler(IMetricsService metricsService)
    {
        this.metricsService = metricsService;
    }

    public Task Handle(OrderShipped notification, CancellationToken cancellationToken)
    {
        this.metricsService.ShipOrders();

        return Task.CompletedTask;
    }
}
