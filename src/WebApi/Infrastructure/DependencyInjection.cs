namespace ButtonShop.WebApi.Infrastructure;

using ButtonShop.WebApi.Domain.Interfaces;
using ButtonShop.WebApi.Infrastructure.Metrics;
using ButtonShop.WebApi.Infrastructure.Services;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IMetricsService, PrometheusMetricsService>();
    }
}
