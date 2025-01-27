using ButtonShop.Application;
using ButtonShop.Infrastructure;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();
app.UseMetricServer();
app.MapControllers();
await app.RunAsync();
