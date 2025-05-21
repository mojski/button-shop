namespace ButtonShop.Infrastructure.HealthChecks.Checks.Lifecycle;

internal sealed class ReadyHealthCheck : IHealthCheck
{
    public static string PATH = "ready";
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {

        HealthCheckResult result = HealthCheckResult.Healthy();
        // TODO
        return Task.FromResult(result);
    }
}
