using ButtonShop.Infrastructure.Monitoring.Elastic;
using HealthStatus = Elastic.Clients.Elasticsearch.HealthStatus;

namespace ButtonShop.Infrastructure.HealthChecks.Checks.Infra;

internal class ElasticSearchHealthCheck : IHealthCheck
{
    public static string PATH = "elasticsearch";
    private readonly IConfiguration configuration;
    private readonly IHttpClientFactory httpClientFactory;

    public ElasticSearchHealthCheck(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        this.configuration = configuration;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var options = configuration.GetSection(ElasticSearchOptions.SECTION_NAME).Get<ElasticSearchOptions>();
        options ??= new ElasticSearchOptions();

        using var client = httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync(options.HealthCheckEndpoint, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var status = JsonSerializer.Deserialize<ElasticSearchHealthStatus>(content, new JsonSerializerOptions
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

    private bool IsHealth(ElasticSearchHealthStatus? status) => status is not null && status.Status is not HealthStatus.Red;

    private record class ElasticSearchHealthStatus
    {
        public HealthStatus Status { get; set; } = HealthStatus.Red;
    }
}
