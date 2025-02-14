using ButtonShop.Application.Events;
using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.Handlers;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;
using ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;
using NSubstitute;

namespace ButtonShop.Infrastructure.UnitTests.Handlers;


[TestClass]
public sealed class OrderShippedHandlerTests
{
    private static string LOG_LEVEL = "INFO";

    private IElasticSearchService elasticSearchService = Substitute.For<IElasticSearchService>();
    private IMetricsService metricsService = Substitute.For<IMetricsService>();
    private OrderShippedHandler? handler;

    [TestInitialize]
    public void Setup()
    {
        this.handler = new OrderShippedHandler(this.metricsService, this.elasticSearchService);
    }

    [TestMethod]
    public async Task Handle_Should_Update_Metrics_And_Send_Event_To_ElasticSearch()
    {
        // Arrange
        var notification = new OrderShipped
        {
            Id = Guid.NewGuid(),
        };
        var cancellationToken = CancellationToken.None;

        // Act
        await this.handler!.Handle(notification, cancellationToken);

        // Assert
        this.metricsService.Received(1).ShipOrders();

        await this.elasticSearchService.Received(1).AddEvent(Arg.Is<BusinessEvent>(e =>
            e.Level == LOG_LEVEL &&
            e.Message == nameof(OrderShipped)));
    }
}
