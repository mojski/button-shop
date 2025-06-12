using ButtonShop.BusinessMetricsService.Metrics.Interfaces;
using StackExchange.Redis;

namespace ButtonShop.BusinessMetricsService.Metrics.Providers;

internal class RedisMetricsProvider : IMetricsProvider
{
    private readonly IDatabase db;
    private readonly IServer server;
    private readonly ILogger<RedisMetricsProvider> logger;

    public RedisMetricsProvider(ILogger<RedisMetricsProvider> logger, IDatabase db, IServer server)
    {
        this.logger = logger;
        this.db = db;
        this.server = server;
    }

    public async Task<int> GetMetric(string pattern, CancellationToken cancellationToken = default)
    {
        int globalSum = 0;

        await foreach (var key in this.server.KeysAsync(pattern: $"*{pattern}"))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                this.logger.LogWarning("Metric retrieval canceled.");
                break;
            }

            var value = await this.db.StringGetAsync(key);

            if (value.HasValue && int.TryParse(value.ToString(), out var parsed))
            {
                globalSum += parsed;
            }
            else
            {
                this.logger.LogWarning("Invalid or missing value for key: {Key}", key);
            }
        }

        return globalSum;
    }
}
