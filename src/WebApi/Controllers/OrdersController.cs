﻿namespace ButtonShop.WebApi.Controllers;

using ButtonShop.Application.Commands;
using ButtonShop.WebApi.Filters;
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
    [ServiceFilter(typeof(ExceptionFilter))]
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
