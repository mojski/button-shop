namespace ButtonShop.Infrastructure.HealthChecks.Checks.Infra;

internal static class DependencyInjection
{
    public static IHealthChecksBuilder AddInfrastructureHealthChecks(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<ElasticSearchHealthCheck>(nameof(ElasticSearchHealthCheck), tags: [HealthCheckConstants.INFRA]);
        builder.AddCheck<OtelHealthCheck>(nameof(OtelHealthCheck), tags: [HealthCheckConstants.INFRA]);
        builder.AddCheck<PersistenceHealthCheck>(nameof(PersistenceHealthCheck), tags: [HealthCheckConstants.INFRA]);
        builder.AddCheck<RedisHealthCheck>(nameof(RedisHealthCheck), tags: [HealthCheckConstants.INFRA]);
        return builder;
    }

    public static void UseInfrastructureHealthChecksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{PersistenceHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(PersistenceHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{OtelHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(OtelHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{ElasticSearchHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(ElasticSearchHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });

        app.MapHealthChecks($"/{HealthCheckConstants.HEALTH_CHECK_ENDPOINT_BASE}/{RedisHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(RedisHealthCheck),
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
        });
    }
}
