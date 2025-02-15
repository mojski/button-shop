using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.Services;
using FluentAssertions;


namespace ButtonShop.Infrastructure.UnitTests.Services;

[TestClass]
public class OrderRepositoryTests
{
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";

    private OrderRepository repository = new OrderRepository();

    [TestMethod]
    public void GetOrder_Should_Return_Order_When_Order_Exists()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), CUSTOMER_NAME, CUSTOMER_ADDRESS);
        this.repository.SaveOrder(order).Wait();

        // Act
        var retrievedOrder = this.repository.GetOrder(order.Id);

        // Assert
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(order);
    }

    [TestMethod]
    public void GetOrder_Should_Return_Null_When_Order_Does_Not_Exist()
    {
        // Act
        var retrievedOrder = this.repository.GetOrder(Guid.NewGuid());

        // Assert
        retrievedOrder.Should().BeNull();
    }

    [TestMethod]
    public async Task SaveOrder_Should_Add_Order()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), CUSTOMER_NAME, CUSTOMER_ADDRESS);

        // Act
        await this.repository.SaveOrder(order);

        // Assert
        var retrievedOrder = this.repository.GetOrder(order.Id);
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(order);
    }

    [TestMethod]
    public async Task ShipOrder_Should_Change_Order_Status_To_Shipped_When_Order_Exists()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), CUSTOMER_NAME, CUSTOMER_ADDRESS);
        await this.repository.SaveOrder(order);

        // Act
        await this.repository.ShipOrder(order.Id);

        // Assert
        order.Status.Should().Be(OrderStatuses.Shipped);
    }

    [TestMethod]
    public async Task ShipOrder_Should_Do_Nothing_When_Order_Does_Not_Exist()
    {
        // Act
        Func<Task> act = async () => await this.repository.ShipOrder(Guid.NewGuid());

        // Assert
        await act.Should().NotThrowAsync();
    }
}