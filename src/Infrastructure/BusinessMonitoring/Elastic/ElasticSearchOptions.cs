namespace ButtonShop.Infrastructure.Monitoring.Elastic;

internal sealed class ElasticSearchOptions
{
    public const string SECTION_NAME = "ElasticSearch";

    public string Address { get; init; } = "http://localhost:9200";
    public string HealthCheckEndpoint  => $"{Address}/_cluster/health";
    public int RequestTimeout { get; set; } = 300;
    public int PingTimeout { get; set; } = 100;
    public int MaxRetries { get; set; } = 0;
    public int DeadTimeout { get; set; } = 10;
}
