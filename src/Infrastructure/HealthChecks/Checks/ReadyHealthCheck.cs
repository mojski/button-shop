namespace ButtonShop.Infrastructure.HealthChecks.Checks;

internal class ReadyHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        HealthCheckResult result = HealthCheckResult.Healthy();
        // TODO 
        return Task.FromResult(result);
    }
}
