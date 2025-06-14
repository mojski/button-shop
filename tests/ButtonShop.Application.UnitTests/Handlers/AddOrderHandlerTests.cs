using ButtonShop.Application.Commands;
using ButtonShop.Application.Events;
using ButtonShop.Application.Handlers;
using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Interfaces;

namespace ButtonShop.Application.UnitTests.Handlers;

[TestClass]
public class AddOrderHandlerTests
{
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";
    private static double LONGITUDE = 50.0;
    private static double LATITUDE = 20.0;
    private static string RED_COLOR = "Red";
    private static string GREEN_COLOR = "Green";
    private static string BLUE_COLOR = "Blue";

    private IOrderRepository repository = Substitute.For<IOrderRepository>();
    private IPublisher mediator = Substitute.For<IPublisher>();
    private NullLogger<AddOrderHandler> logger = new();
    private AddOrderHandler? handler;

    [TestInitialize]
    public void Setup()
    {
        this.handler = new AddOrderHandler(this.repository, this.mediator, this.logger);
    }

    [TestMethod]
    public async Task Handle_ShouldSaveOrderAndPublishNotification()
    {
        // Arrange
        var request = new AddOrder
        {
            Id = Guid.NewGuid(),
            CustomerName = CUSTOMER_NAME,
            ShippingAddress = CUSTOMER_ADDRESS,
            Items = new Dictionary<string, int>
            {
                { RED_COLOR, 2 },
                { GREEN_COLOR, 3 },
                { BLUE_COLOR, 4 }
            },
            Longitude = LONGITUDE,
            Latitude = LATITUDE,
        };

        OrderAdded? notificationSent = null;
        Order? orderSaved = null;

        await this.mediator
            .Publish(Arg.Do<OrderAdded>(notification => notificationSent = notification), Arg.Any<CancellationToken>());

        await this.repository
            .SaveOrder(Arg.Do<Order>(order => orderSaved = order), Arg.Any<CancellationToken>());

        // Act
        await handler!.Handle(request, CancellationToken.None);

        // Assert
        var expectedOrder = new Order(request.Id, request.CustomerName, request.ShippingAddress);
        expectedOrder.AddItems(RED_COLOR, 2);
        expectedOrder.AddItems(GREEN_COLOR, 3);
        expectedOrder.AddItems(BLUE_COLOR, 4);

        var expectedNotification = new OrderAdded
        {
            Id = request.Id,
            Items = new Dictionary<ButtonColors, int>
            {
                { ButtonColors.Red, 2 },
                { ButtonColors.Green, 3 },
                { ButtonColors.Blue, 4 }
            },
            Longitude = request.Longitude,
            Latitude = request.Latitude,
        };

        notificationSent.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;

        orderSaved.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedOrder)
            ;
    }
}
