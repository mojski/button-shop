namespace ButtonShop.Infrastructure.HealthChecks.Checks.Lifecycle;

internal class LivenessHealthCheck : IHealthCheck
{
    public static string PATH = "liveness";
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {

        HealthCheckResult result = HealthCheckResult.Healthy();
        // TODO
        return Task.FromResult(result);
    }
}
