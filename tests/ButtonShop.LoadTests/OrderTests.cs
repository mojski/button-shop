using System.Net.Http.Json;
using ButtonShop.Application.Commands;
using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;

namespace ButtonShop.LoadTests;

internal static class OrderTests
{
    private static string SCENARIO_NAME = "add_order_scenario";
    private static string ADD_ORDER_ENDPOINT = "/orders/add";
    private static string SHIP_ORDER_ENDPOINT = "/orders/ship";
    private static string CUSTOMER_NAME = "CustomerName";
    private static string CUSTOMER_ADDRESS = "Address";
    private static double LONGITUDE = 50.0;
    private static double LATITUDE = 20.0;
    private static string RED_COLOR = "Red";
    private static string GREEN_COLOR = "Green";
    private static string BLUE_COLOR = "Blue";

    public static async Task Run(string baseUrl, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(baseUrl);

        var clientArgs = HttpClientArgs.Create(cancellationToken);


        var scenario = Scenario.Create(SCENARIO_NAME, async context =>
        {
            var order_step = await Step.Run("order_buttons", context, async () =>
            {
                var orderId = Guid.NewGuid();
                context.Data["order_id"] = orderId;

                var order = GetOrderPayload(orderId).AsJsonContent();

                var request =
                    Http.CreateRequest("POST", ADD_ORDER_ENDPOINT)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(order);

                var response = await Http.Send(httpClient, clientArgs, request);
                return response;
            });

            var shipment_step = await Step.Run("ship_button", context, async () =>
            {
                var orderId = (Guid)context.Data["order_id"];

                var ship = GetShipPayload(orderId).AsJsonContent();

                var request =
                    Http.CreateRequest("POST", SHIP_ORDER_ENDPOINT)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(ship);

                var response = await Http.Send(httpClient, clientArgs, request);
                return response;
            });

            return Response.Ok();
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(10))
        .WithLoadSimulations(Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(30)));

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }

    private static AddOrder GetOrderPayload(Guid id) =>  new AddOrder
    {
        Id = id,
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

    private static Ship GetShipPayload(Guid id) => new Ship { OrderId = id };

    private static JsonContent AsJsonContent(this object input) => JsonContent.Create(input);
}
