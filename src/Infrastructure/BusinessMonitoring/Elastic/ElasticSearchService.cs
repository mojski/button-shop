namespace ButtonShop.Infrastructure.Monitoring.Elastic;

using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;
using global::Elastic.Clients.Elasticsearch;

internal sealed class ElasticSearchService : IElasticSearchService
{
    private readonly ElasticSearchOptions options;

    public ElasticSearchService(ElasticSearchOptions options)
    {
        this.options = options;
    }

    public async Task AddEvent(BusinessEvent businessEvent)
    {
        var elasticUri = new Uri(this.options.Address);

        var settings = new ElasticsearchClientSettings(elasticUri)
            .DefaultIndex(ElasticConstants.EVENT_INDEX);

        var client = new ElasticsearchClient(settings);

        await client.Indices.CreateAsync(ElasticConstants.EVENT_INDEX, cfg =>
        {
            cfg.Mappings(map => map.Properties<BusinessEvent>(p => p
                .Keyword(k => k.Id!)
                .Text(t => t.Message)
                .Text(t => t.Level)
                .Date(d => d.Timestamp)));
        });

        await client.IndexAsync(businessEvent);
    }

    public async Task AddGeoLocationStat(OrderGeoLoc location)
    {
        var elasticUri = new Uri(this.options.Address);

        var settings = new ElasticsearchClientSettings(elasticUri)
            .DefaultIndex(ElasticConstants.LOCATION_INDEX);

        var client = new ElasticsearchClient(settings);

        await client.Indices.CreateAsync(ElasticConstants.LOCATION_INDEX, cfg =>
        {
            cfg.Mappings(map => map.Properties<OrderGeoLoc>(p => p
                .Keyword(k => k.Id!)
                .Text(t => t.Longitude)
                .Text(t => t.Latitude)
                .IntegerNumber(n => n.Quantity)
                .GeoPoint(g => g.GeoLocation)
                .Date(d => d.Timestamp)));
        });

        await client.IndexAsync(location);
    }

    private Task CreateIndexes()
    {
        return Task.CompletedTask;
    }
}
