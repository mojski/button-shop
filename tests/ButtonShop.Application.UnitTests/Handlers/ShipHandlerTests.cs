using ButtonShop.Application.Commands;
using ButtonShop.Application.Events;
using ButtonShop.Application.Handlers;
using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;

namespace ButtonShop.Application.UnitTests.Handlers;

[TestClass]
public class ShipHandlerTests
{
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";

    private IOrderRepository repository = Substitute.For<IOrderRepository>();
    private IPublisher mediator = Substitute.For<IPublisher>();
    private NullLogger<ShipHandler> logger = new();
    private ShipHandler? handler;

    [TestInitialize]
    public void Setup()
    {
        this.handler = new ShipHandler(this.repository, this.mediator, this.logger);
    }

    [TestMethod]
    public async Task Handle_ShouldSaveOrderAndPublishNotification()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order(orderId, CUSTOMER_NAME, CUSTOMER_ADDRESS);

        var request = new Ship
        { 
            OrderId = orderId,
        };

        this.repository
            .GetOrder(Arg.Is(orderId))
            .Returns(order)
            ;

        OrderShipped? notificationSent = null;
        Order? shipped = null;

        await this.mediator
            .Publish(Arg.Do<OrderShipped>(notification => notificationSent = notification), Arg.Any<CancellationToken>());

        await this.repository
            .SaveOrder(Arg.Do<Order>(order => shipped = order), Arg.Any<CancellationToken>());

        // Act
        await handler!.Handle(request, CancellationToken.None);

        // Assert
        var expectedNotification = new OrderShipped
        {
            Id = request.OrderId,
        };

        notificationSent.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;

        shipped!.Status.Should()
            .Be(OrderStatuses.Shipped)
            ;
    }
}
