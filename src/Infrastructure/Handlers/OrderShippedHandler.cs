namespace ButtonShop.Infrastructure.Handlers;

using ButtonShop.Application.Events;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;

internal sealed class OrderShippedHandler : INotificationHandler<OrderShipped>
{
    private static string DEFAULT_LOG_LEVEL = "INFO";

    private readonly IElasticSearchService elasticSearchService;
    private readonly IMetricsService metricsService;
    private readonly ILogger<OrderShippedHandler> logger;

    public OrderShippedHandler(IMetricsService metricsService, IElasticSearchService elasticSearchService, ILogger<OrderShippedHandler> logger)
    {
        this.metricsService = metricsService;
        this.elasticSearchService = elasticSearchService;
        this.logger = logger;
    }

    public async Task Handle(OrderShipped notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("OrderShipped handle for id {id}", notification.Id);
        
        await this.metricsService.AddShipmentMetrics(cancellationToken);

        var @event = new BusinessEvent
        {
            Level = DEFAULT_LOG_LEVEL,
            Message = nameof(OrderShipped),
        };

        await this.elasticSearchService.AddEvent(@event);
    }
}
