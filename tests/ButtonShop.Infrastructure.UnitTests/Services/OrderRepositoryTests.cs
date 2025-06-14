using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.Persistence.Models;
using ButtonShop.Infrastructure.Persistence.Services;

namespace ButtonShop.Infrastructure.UnitTests.Services;

[TestClass]
public class OrderRepositoryTests
{
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";
    private static Guid ORDER_ID = Guid.Parse("5f844d79-91c8-4ff0-8062-b479e9ad725b");
    private NullLogger<OrderRepository> logger = new();
    private readonly IDocumentStore documentStore = Substitute.For<IDocumentStore>();

    private OrderRepository repository;
    private readonly Order orderEntity;
    private readonly OrderDbModel dbModel;

    public OrderRepositoryTests()
    {
        this.repository = new OrderRepository(this.documentStore, this.logger);

        this.dbModel = new OrderDbModel
        {
            Id = ORDER_ID,
            CustomerName = CUSTOMER_NAME,
            ShippingAddress = CUSTOMER_ADDRESS,
            Status = 0,
            Items = new Dictionary<int, int>
            {
                { (int)ButtonColors.Red, 2 },
                { (int)ButtonColors.Green, 3 },
                { (int)ButtonColors.Blue, 7 },
            }
        };

        this.orderEntity = new Order(ORDER_ID, CUSTOMER_NAME, CUSTOMER_ADDRESS);
        orderEntity.AddItems(ButtonColors.Red, 2);
        orderEntity.AddItems(ButtonColors.Green, 3);
        orderEntity.AddItems(ButtonColors.Blue, 7);
    }

    [TestMethod]
    public async Task GetOrder_Should_Return_Order_When_Order_Exists()
    {
        // Arrange
        var session = Substitute.For<IQuerySession>();

        session.LoadAsync<OrderDbModel>(ORDER_ID, Arg.Any<CancellationToken>())
       .Returns(this.dbModel);

        this.documentStore.QuerySession().Returns(session);

        // Act
        var retrievedOrder = await this.repository.GetOrder(ORDER_ID);

        // Assert
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(this.orderEntity);
    }

    [TestMethod]
    public async Task SaveOrder_Should_Add_Order()
    {
        // Arrange
        var session = Substitute.For<IDocumentSession>();
        this.documentStore.LightweightSession().Returns(session);

        OrderDbModel? savedOrder = null;

        session.When(s => s.Store<OrderDbModel>(Arg.Any<OrderDbModel[]>()))
               .Do(call =>
               {
                   var array = call.Arg<OrderDbModel[]>();
                   savedOrder = array.FirstOrDefault();
               });

        // Act
        await this.repository.SaveOrder(this.orderEntity);

        // Assert
        savedOrder.Should().BeEquivalentTo(this.dbModel);
        await session.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}