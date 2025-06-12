
using StackExchange.Redis;

namespace ButtonShop.Infrastructure.HealthChecks.Checks.Infra;

internal class RedisHealthCheck : IHealthCheck
{
    public static string PATH = "redis";

    private readonly IDatabase database;

    public RedisHealthCheck(IConnectionMultiplexer connectionMultiplexer)
    {
        this.database = connectionMultiplexer.GetDatabase();
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var pong = await this.database.PingAsync();
            return HealthCheckResult.Healthy($"Redis responded in {pong.TotalMilliseconds} ms");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis unreachable", ex);
        }
    }
}
