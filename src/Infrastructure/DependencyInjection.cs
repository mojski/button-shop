namespace ButtonShop.Infrastructure;

using ButtonShop.Domain.Interfaces;
using ButtonShop.Infrastructure.BusinessMonitoring;
using ButtonShop.Infrastructure.OpenTelemetry;
using ButtonShop.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddBusinessMonitoring(configuration);
        services.AddOpenTelemetry(configuration);
    }
}
