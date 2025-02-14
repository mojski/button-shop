using ButtonShop.Application.Commands;
using ButtonShop.Application.Events;
using ButtonShop.Application.Handlers;
using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace ButtonShop.Application.UnitTests.Handlers;

[TestClass]
public class ShipHandlerTests
{
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";

    private IOrderRepository repository = Substitute.For<IOrderRepository>();
    private IPublisher mediator = Substitute.For<IPublisher>();
    private ShipHandler? handler;

    [TestInitialize]
    public void Setup()
    {
        this.handler = new ShipHandler(this.repository, this.mediator);
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
        Guid orderIdShipped = Guid.Empty;

        await this.mediator
            .Publish(Arg.Do<OrderShipped>(notification => notificationSent = notification), Arg.Any<CancellationToken>());

        await this.repository
            .ShipOrder(Arg.Do<Guid>(orderId => orderIdShipped = orderId), Arg.Any<CancellationToken>());

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

        orderIdShipped.Should()
            .Be(request.OrderId)
            ;
    }
}
