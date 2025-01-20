namespace ButtonShop.WebApi.Infrastructure.Handlers;

using ButtonShop.WebApi.Application.Events;
using ButtonShop.WebApi.Domain.Entities;
using ButtonShop.WebApi.Infrastructure.Metrics;
using MediatR;

public sealed class OrderAddedHandler : INotificationHandler<OrderAdded>
{
    private readonly IMetricsService metricsService;

    public OrderAddedHandler(IMetricsService metricsService)
    {
        this.metricsService = metricsService;
    }

    public Task Handle(OrderAdded notification, CancellationToken cancellationToken)
    {
        this.metricsService.AddOrder();
        this.metricsService.SellRed(notification.Items[ButtonColors.Red]);
        this.metricsService.SellGreen(notification.Items[ButtonColors.Green]);
        this.metricsService.SellBlue(notification.Items[ButtonColors.Blue]);

        return Task.CompletedTask;
    }
}
