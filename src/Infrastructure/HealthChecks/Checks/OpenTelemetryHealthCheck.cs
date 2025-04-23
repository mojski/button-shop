using ButtonShop.Infrastructure.OpenTelemetry;
using System.Text.Json;
using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;

namespace ButtonShop.Infrastructure.HealthChecks.Checks;

internal class OpenTelemetryHealthCheck : IHealthCheck
{
    public static string PATH = "open_telemetry_health_check";
    private static string desiredHealthStatus = "Server available";
    private readonly IConfiguration configuration;
    private readonly IHttpClientFactory httpClientFactory;

    public OpenTelemetryHealthCheck(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        this.configuration = configuration;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var options = configuration.GetSection(OpenTelemetryOptions.SECTION_NAME).Get<OpenTelemetryOptions>();
        options ??= new OpenTelemetryOptions();

        var host = options.HealthCheckEndpoint;

        using var client = httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync(host, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var status = JsonSerializer.Deserialize<OtelStatus>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var healthStatus = string.Equals(status?.Status, desiredHealthStatus, StringComparison.InvariantCultureIgnoreCase) ? HealthStatus.Healthy : HealthStatus.Unhealthy;

            return new HealthCheckResult(healthStatus);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Error: {ex.Message}");
        }
    }

    private record class OtelStatus
    {
        public string Status { get; set; } = string.Empty;
    }
}
