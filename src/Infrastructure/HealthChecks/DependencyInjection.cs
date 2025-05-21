using ButtonShop.Infrastructure.HealthChecks.Checks.Infra;
using ButtonShop.Infrastructure.HealthChecks.Checks.Lifecycle;
using ButtonShop.Infrastructure.HealthChecks.Checks.System;

namespace ButtonShop.Infrastructure.HealthChecks;

internal static class DependencyInjection
{
    public static void AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddHealthChecks()
            .AddHostSystemHealthChecks()
            .AddLifecycleChecks()
            .AddInfrastructureHealthChecks();
    }

    public static void UseHealthChecksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}", new HealthCheckOptions()
        {
            Predicate = check => !check.Tags.Contains(HealthCheckConstants.LIFECYCLE),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.UseHostSystemHealthChecksEndpoints();
        app.UseLifecycleChecksEndpoints();
        app.UseInfrastructureHealthChecksEndpoints();
    }
}
