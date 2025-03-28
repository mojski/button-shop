using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ButtonShop.Infrastructure.HealthChecks.Checks;

internal sealed class PersistenceHealthCheck : IHealthCheck
{
    public static string PATH = "persistence_health_check";

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = HealthCheckResult.Healthy();
        // TODO implement check after persistence
        return Task.FromResult(result);
    }
}
