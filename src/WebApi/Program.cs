using ButtonShop.Application;
using ButtonShop.Infrastructure;
using ButtonShop.Infrastructure.OpenTelemetry;
using ButtonShop.WebApi.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("trace_id", ctx.HttpContext.TraceIdentifier);
            ctx.ProblemDetails.Extensions.Add("request", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
            ctx.ProblemDetails.Extensions.Add("service_name", "ButtonShop Api");
        });

builder.Services.AddExceptionHandler<ExceptionsHandler>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

builder.Services.AddApplication();
builder.Logging.UseOpenTelemetry(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseExceptionHandler();
app.UseMetricServer();
app.UseInfrastructure();
app.MapControllers();
await app.RunAsync();
