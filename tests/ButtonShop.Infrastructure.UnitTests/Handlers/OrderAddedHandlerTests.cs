using ButtonShop.Application.Events;
using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using ButtonShop.Infrastructure.Handlers;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace ButtonShop.Infrastructure.UnitTests.Handlers;

[TestClass]
public sealed class OrderAddedHandlerTests
{
    private static double LONGITUDE = 50.0;
    private static double LATITUDE = 20.0;
    private static string LOG_LEVEL = "INFO";

    private IElasticSearchService elasticSearchService = Substitute.For<IElasticSearchService>();
    private IMetricsService metricsService = Substitute.For<IMetricsService>();
    private NullLogger<OrderAddedHandler> logger = new();
    private OrderAddedHandler? handler;

    [TestInitialize]
    public void Setup()
    {
        this.handler = new OrderAddedHandler(this.metricsService, this.elasticSearchService, this.logger);
    }

    [TestMethod]
    public async Task Handle_Should_Update_Metrics_And_Send_Data_To_ElasticSearch()
    {
        // Arrange
        var notification = new OrderAdded
        {
            Id = Guid.NewGuid(),
            Items = new Dictionary<ButtonColors, int>
            {
                { ButtonColors.Red, 2 },
                { ButtonColors.Green, 3 },
                { ButtonColors.Blue, 4 }
            },
            Longitude = LONGITUDE,
            Latitude = LATITUDE,
        };

        var cancellationToken = CancellationToken.None;

        // Act
        await this.handler!.Handle(notification, cancellationToken);

        // Assert
        await this.metricsService.Received(1).AddOrderMetrics(notification);

        await this.elasticSearchService.Received(1).AddGeoLocationStat(Arg.Is<OrderGeoLoc>(geo =>
            geo.Longitude == LONGITUDE &&
            geo.Latitude == LATITUDE &&
            geo.Quantity == 9));

        await this.elasticSearchService.Received(1).AddEvent(Arg.Is<BusinessEvent>(e =>
            e.Level == LOG_LEVEL &&
            e.Message == nameof(OrderAdded)));
    }
}
