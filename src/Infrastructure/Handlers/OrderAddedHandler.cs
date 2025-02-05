namespace ButtonShop.Infrastructure.Handlers;

using ButtonShop.Application.Events;
using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;
using ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;
using MediatR;

public sealed class OrderAddedHandler : INotificationHandler<OrderAdded>
{
    private readonly IElasticSearchService elasticSearchService;
    private readonly IMetricsService metricsService;

    public OrderAddedHandler(IMetricsService metricsService, IElasticSearchService elasticSearchService)
    {
        this.metricsService = metricsService;
        this.elasticSearchService = elasticSearchService;
    }

    public async Task Handle(OrderAdded notification, CancellationToken cancellationToken)
    {
        this.metricsService.AddOrder();
        this.metricsService.SellRed(notification.Items[ButtonColors.Red]);
        this.metricsService.SellGreen(notification.Items[ButtonColors.Green]);
        this.metricsService.SellBlue(notification.Items[ButtonColors.Blue]);

        var geolocation = new OrderGeoLoc
        {
            Longitude = notification.Longitude,
            Latitude = notification.Latitude,
            Quantity = notification.Items.Sum(item => item.Value),
        };

        var @event = new BusinessEvent
        {
            Level = "info",
            Message = nameof(OrderAdded),
        };

        await this.elasticSearchService.AddGeoLocationStat(geolocation);
        //await service.AddEvent(@event);
    }
}
