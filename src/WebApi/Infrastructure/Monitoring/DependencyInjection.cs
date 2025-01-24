namespace ButtonShop.WebApi.Infrastructure.Monitoring;

using ButtonShop.WebApi.Infrastructure.Monitoring.Elastic;
using ButtonShop.WebApi.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.WebApi.Infrastructure.Monitoring.Metrics;
using ButtonShop.WebApi.Infrastructure.Monitoring.Metrics.Interfaces;

public static class DependencyInjection
{
    public static void AddBusinessMonitoring(this IServiceCollection services)
    {
        services.AddSingleton<IMetricsService, PrometheusMetricsService>();
        services.AddSingleton<ElasticSearchOptions>();
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
    }
}
