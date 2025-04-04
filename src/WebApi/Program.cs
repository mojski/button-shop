using ButtonShop.Application;
using ButtonShop.Infrastructure;
using ButtonShop.Infrastructure.OpenTelemetry;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Logging.UseOpenTelemetry(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseMetricServer();
app.UseInfrastructure();
app.MapControllers();
await app.RunAsync();
