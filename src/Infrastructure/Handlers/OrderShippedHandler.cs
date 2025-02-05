namespace ButtonShop.Infrastructure.Handlers;

using ButtonShop.Application.Events;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;
using ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;
using MediatR;

public sealed class OrderShippedHandler : INotificationHandler<OrderShipped>
{
    private readonly IElasticSearchService elasticSearchService;
    private readonly IMetricsService metricsService;

    public OrderShippedHandler(IMetricsService metricsService, IElasticSearchService elasticSearchService)
    {
        this.metricsService = metricsService;
        this.elasticSearchService = elasticSearchService;
    }

    public async Task Handle(OrderShipped notification, CancellationToken cancellationToken)
    {
        this.metricsService.ShipOrders();

        var @event = new BusinessEvent
        {
            Level = "info",
            Message = nameof(OrderShipped),
        };

        await this.elasticSearchService.AddEvent(@event);
    }
}
