namespace ButtonShop.Infrastructure.HealthChecks.Checks.System;

internal static class DependencyInjection
{
    public static IHealthChecksBuilder AddHostSystemHealthChecks(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<MemoryHealthCheck>(nameof(MemoryHealthCheck), tags: [HealthCheckConstants.SYSTEM]);
        return builder;
    }

    public static void UseHostSystemHealthChecksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{HealthCheckConstants.SYSTEM}", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(HealthCheckConstants.SYSTEM),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });
    }
}
