namespace ButtonShop.WebApi.Controllers;

using ButtonShop.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]/[action]")]
[ApiController]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender mediator;
    private readonly ILogger<OrdersController> logger;

    public OrdersController(ISender mediator, ILogger<OrdersController> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddOrder command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);
        this.logger.LogInformation("Order added: {Id}", command.Id);

        return this.Accepted();
    }

    [HttpPost]
    public async Task<IActionResult> Ship([FromBody] Ship command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);
        this.logger.LogInformation("Order added: {OrderId}", command.OrderId);
        return this.Accepted();
    }
}
