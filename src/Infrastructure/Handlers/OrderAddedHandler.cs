﻿namespace ButtonShop.Infrastructure.Handlers;

using ButtonShop.Application.Events;
using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;
using MediatR;
using Microsoft.Extensions.Logging;

internal sealed class OrderAddedHandler : INotificationHandler<OrderAdded>
{
    private static string DEFAULT_LOG_LEVEL = "INFO";
    private readonly IElasticSearchService elasticSearchService;
    private readonly IMetricsService metricsService;
    private readonly ILogger<OrderAddedHandler> logger;

    public OrderAddedHandler(IMetricsService metricsService, IElasticSearchService elasticSearchService, ILogger<OrderAddedHandler> logger)
    {
        this.metricsService = metricsService;
        this.elasticSearchService = elasticSearchService;
        this.logger = logger;
    }

    public async Task Handle(OrderAdded notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("OrderAdded handle for id {id}", notification.Id);

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
            Level = DEFAULT_LOG_LEVEL,
            Message = nameof(OrderAdded),
        };

        
        await this.elasticSearchService.AddGeoLocationStat(geolocation);
        await this.elasticSearchService.AddEvent(@event);
    }
}
