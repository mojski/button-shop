using System.Net;
using System.Net.Http.Json;
using ButtonShop.Application.Commands;
using ButtonShop.IntegrationTests.Setup;

namespace ButtonShop.IntegrationTests.Tests;

[TestClass]
public sealed class OrderControllerTests : IDisposable
{
    private const string ADD_ORDER_ENDPOINT = "/orders/add";
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";
    private static double LONGITUDE = 50.0;
    private static double LATITUDE = 20.0;
    private static string RED_COLOR = "Red";
    private static string GREEN_COLOR = "Green";
    private static string BLUE_COLOR = "Blue";

    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;
    private readonly AddOrder order;


    public OrderControllerTests()
    {
        this.factory = new CustomWebApplicationFactory<Program>();
        this.httpClient = factory.CreateDefaultClient();
        this.order = new AddOrder
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
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }

    [TestMethod]
    public async Task Should_Return_Success_Status_Code()
    {
        // Arrange
        var payload = JsonContent.Create(this.order);

        // Act
        var response = await this.httpClient.PostAsync(ADD_ORDER_ENDPOINT, payload, CancellationToken.None);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Accepted)
            ;
    }

    [TestMethod]
    public async Task Should_Return_Bad_Request_Status_Code_For_Invalid_Payload()
    {
        // Act
        var invalidOrder = this.order with { Latitude = 91 };
        var payload = JsonContent.Create(invalidOrder);

        // Act
        var response = await this.httpClient.PostAsync(ADD_ORDER_ENDPOINT, payload, CancellationToken.None);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest)
            ;
    }
}
