using ButtonShop.Infrastructure.Monitoring.Elastic;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Metrics;
using ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ButtonShop.Infrastructure.Monitoring;

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
