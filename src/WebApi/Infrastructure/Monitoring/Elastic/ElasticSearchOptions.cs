namespace ButtonShop.WebApi.Infrastructure.Monitoring.Elastic;

public sealed class ElasticSearchOptions
{
    public const string SECTION_NAME = "ElasticSearch";

    public string Address { get; init; } = "http://localhost:9200";
}
