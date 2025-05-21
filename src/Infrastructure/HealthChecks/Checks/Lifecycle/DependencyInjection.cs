namespace ButtonShop.Infrastructure.HealthChecks.Checks.Lifecycle;

internal static class DependencyInjection
{
    public static IHealthChecksBuilder AddLifecycleChecks(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<ReadyHealthCheck>(nameof(ReadyHealthCheck), tags: [HealthCheckConstants.LIFECYCLE]);
        builder.AddCheck<StartupHealthCheck>(nameof(StartupHealthCheck), tags: [HealthCheckConstants.LIFECYCLE]);
        builder.AddCheck<LivenessHealthCheck>(nameof(LivenessHealthCheck), tags: [HealthCheckConstants.LIFECYCLE]);
        return builder;
    }

    public static void UseLifecycleChecksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{ReadyHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(ReadyHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{StartupHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(StartupHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{StartupHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(StartupHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });
    }
}
