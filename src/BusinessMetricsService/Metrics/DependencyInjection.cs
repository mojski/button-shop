using ButtonShop.BusinessMetricsService.Metrics.Interfaces;
using ButtonShop.BusinessMetricsService.Metrics.Providers;
using ButtonShop.Common.BusinessMetrics;
using StackExchange.Redis;

namespace ButtonShop.BusinessMetricsService.Metrics;

internal static class DependencyInjection
{
    public static void AddBusinessMetricCollection(this IServiceCollection services, IConfiguration configuration)
    {
        var redisOptions = configuration.GetSection(RedisOptions.SECTION_NAME).Get<RedisOptions>()
                           ?? new RedisOptions();

        var connectionMultiplexer = ConnectionMultiplexer.Connect(redisOptions.Connection);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

        var database = connectionMultiplexer.GetDatabase();
        var server = connectionMultiplexer.GetServer(redisOptions.Connection);

        services.AddSingleton(database);
        services.AddSingleton(server);
        services.AddSingleton<IMetricsProvider, RedisMetricsProvider>();
        services.AddSingleton<MetricsCollector>();
    }
}
