namespace ButtonShop.Infrastructure.HealthChecks.Checks.Lifecycle;

internal sealed class StartupHealthCheck : IHealthCheck
{
    public static string PATH = "startup";
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        HealthCheckResult result = HealthCheckResult.Healthy();
        // TODO
        return Task.FromResult(result);
    }
}
