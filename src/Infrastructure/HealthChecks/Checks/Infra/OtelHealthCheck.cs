using ButtonShop.Infrastructure.OpenTelemetry;

namespace ButtonShop.Infrastructure.HealthChecks.Checks.Infra;

internal class OtelHealthCheck : IHealthCheck
{
    public static string PATH = "otel";
    private static string desiredHealthStatus = "Server available";
    private readonly IConfiguration configuration;
    private readonly IHttpClientFactory httpClientFactory;

    public OtelHealthCheck(IConfiguration configuration, IHttpClientFactory httpClientFactory)
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

            if (IsHealth(status))
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Error: {ex.Message}");
        }
    }

    private bool IsHealth(OtelStatus? status)
    {
        return status is not null && string.Equals(status.Status, desiredHealthStatus, StringComparison.InvariantCultureIgnoreCase);
    }

    private record class OtelStatus
    {
        public string Status { get; set; } = string.Empty;
    }
}
