namespace ButtonShop.WebApi.Controllers;

using ButtonShop.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]/[action]")]
[ApiController]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender mediator;

    public OrdersController(ISender mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddOrder command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }

    [HttpPost]
    public async Task<IActionResult> Ship([FromBody] Ship command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);

        return this.Accepted();
    }
}
