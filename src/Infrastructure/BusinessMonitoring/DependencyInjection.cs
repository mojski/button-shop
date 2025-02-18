using ButtonShop.Infrastructure.Monitoring.Elastic;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ButtonShop.Infrastructure.BusinessMonitoring;

public static class DependencyInjection
{
    public static void AddBusinessMonitoring(this IServiceCollection services, IConfiguration configuration)
    {
        var elasticSearchOptions = configuration.GetSection(ElasticSearchOptions.SECTION_NAME).Get<ElasticSearchOptions>();
        elasticSearchOptions ??= new ElasticSearchOptions();

        services.AddSingleton<IMetricsService, PrometheusMetricsService>();
        services.AddSingleton(elasticSearchOptions);
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
    }
}
