using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ButtonShop.Infrastructure.HealthChecks.Checks;

internal class StartupHealthCheck : IHealthCheck
{
    private readonly IServiceProvider serviceProvider;

    public StartupHealthCheck(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        HealthCheckResult result = HealthCheckResult.Healthy();

        return Task.FromResult(result);
    }
}
