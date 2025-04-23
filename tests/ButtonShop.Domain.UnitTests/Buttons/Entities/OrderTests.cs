using ButtonShop.Domain.Entities;
using ButtonShop.Domain.Exceptions;

namespace ButtonShop.Domain.UnitTests.Buttons.Entities;

[TestClass]
public class OrderTests
{
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";
    private static string INVALID_COLOR = "Deep Purple";

    private static readonly Guid id = Guid.NewGuid();

    private Order defaultOrder = new Order(id, CUSTOMER_NAME, CUSTOMER_ADDRESS);

    [TestMethod]
    public void Constructor_ShouldInitializeOrderWithCorrectValues()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var order = new Order(id, CUSTOMER_NAME, CUSTOMER_ADDRESS);

        // Assert
        order.Id.Should().Be(id);
        order.CustomerName.Should().Be(CUSTOMER_NAME);
        order.ShippingAddress.Should().Be(CUSTOMER_ADDRESS);
        order.Status.Should().Be(OrderStatuses.Awaiting);
        order.Items.Should().NotBeNull().And.BeEmpty();
    }

    [TestMethod]
    public void AddItems_Should_Add_Items_Correctly()
    {
        // Act
        defaultOrder.AddItems("Red", 2);
        defaultOrder.AddItems("Red", 3);
        defaultOrder.AddItems("Blue", 5);

        // Assert
        defaultOrder.Items.Should().HaveCount(2);
        defaultOrder.Items[ButtonColors.Red].Should().Be(5);
        defaultOrder.Items[ButtonColors.Blue].Should().Be(5);
    }

    [TestMethod]
    public void AddItems_Should_Throw_Exception_For_Invalid_Color()
    {
        // Act
        Action act = () => defaultOrder.AddItems(INVALID_COLOR, 1);

        // Assert
        act.Should().Throw<ButtonColorNotSupportedException>().WithMessage($"*{INVALID_COLOR}*");
    }

    [TestMethod]
    public void Ship_ShouldChangeStatusToShipped()
    {
        // Act
        defaultOrder.Ship();

        // Assert
        defaultOrder.Status.Should().Be(OrderStatuses.Shipped);
    }
}
