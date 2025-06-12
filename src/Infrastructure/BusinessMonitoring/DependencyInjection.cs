using ButtonShop.Infrastructure.Monitoring.Elastic;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using ButtonShop.Common.BusinessMetrics;
using StackExchange.Redis;

namespace ButtonShop.Infrastructure.BusinessMonitoring;

public static class DependencyInjection
{
    public static void AddBusinessMonitoring(this IServiceCollection services, IConfiguration configuration)
    {
        var elasticSearchOptions = configuration.GetSection(ElasticSearchOptions.SECTION_NAME).Get<ElasticSearchOptions>();
        elasticSearchOptions ??= new ElasticSearchOptions();

        var redisOptions = configuration.GetSection(RedisOptions.SECTION_NAME).Get<RedisOptions>();
        redisOptions ??= new RedisOptions();

        services.AddSingleton<IConnectionMultiplexer>(sp => 
            ConnectionMultiplexer.Connect(redisOptions.Connection));

        services.AddSingleton(elasticSearchOptions);
        services.AddSingleton<IMetricsService, MetricsAggregationService>();
        services.AddSingleton<BusinessMetricRegistry>();
        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
    }
}
