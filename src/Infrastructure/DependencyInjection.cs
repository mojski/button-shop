namespace ButtonShop.Infrastructure;

using ButtonShop.Domain.Interfaces;
using ButtonShop.Infrastructure.Monitoring;
using ButtonShop.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddBusinessMonitoring();
    }
}
