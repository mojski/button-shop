using ButtonShop.Infrastructure.HealthChecks.Checks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ButtonShop.Infrastructure.HealthChecks;

internal static class DependencyInjection
{
    private static string HEALTH_CHECK_ENDPOINT_BASE = "health";

    private static Func<HttpContext, HealthReport, Task> DefaultResponseWriter = HealthCheckResponseWriter.WriteResponse;

    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddHealthChecks()
            .AddCheck<MemoryHealthCheck>(nameof(MemoryHealthCheck))
            .AddCheck<OpenTelemetryHealthCheck>(nameof(OpenTelemetryHealthCheck))
            .AddCheck<PersistenceHealthCheck>(nameof(PersistenceHealthCheck));

        return services;
    }

    public static void UseHealthChecksEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks($"/{HEALTH_CHECK_ENDPOINT_BASE}", new HealthCheckOptions()
        {
            ResponseWriter = DefaultResponseWriter,
        });

        app.MapHealthChecks($"/{HEALTH_CHECK_ENDPOINT_BASE}/{PersistenceHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(PersistenceHealthCheck),
            ResponseWriter = DefaultResponseWriter,
        });

        app.MapHealthChecks($"/{HEALTH_CHECK_ENDPOINT_BASE}/{MemoryHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(MemoryHealthCheck),
            ResponseWriter = DefaultResponseWriter,
        });

        app.MapHealthChecks($"/{HEALTH_CHECK_ENDPOINT_BASE}/{OpenTelemetryHealthCheck.PATH}", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Name == nameof(OpenTelemetryHealthCheck),
            ResponseWriter = DefaultResponseWriter,
        });
    }
}
