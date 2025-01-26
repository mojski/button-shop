using ButtonShop.Infrastructure.Monitoring.Elastic;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Metrics;
using ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ButtonShop.Infrastructure.Monitoring;

public static class DependencyInjection
{
    public static void AddBusinessMonitoring(this IServiceCollection services)
    {
        services.AddSingleton<IMetricsService, PrometheusMetricsService>();
        services.AddSingleton<ElasticSearchOptions>();
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
    }
}
