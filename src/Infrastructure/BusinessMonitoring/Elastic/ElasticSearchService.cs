namespace ButtonShop.Infrastructure.Monitoring.Elastic;

using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;

internal sealed class ElasticSearchService : IElasticSearchService
{
    private readonly ElasticsearchClient client;

    public ElasticSearchService(ElasticSearchOptions options)
    {
        var elasticUri = new Uri(options.Address);
        var clientSettings = new ElasticsearchClientSettings(elasticUri)
            .RequestTimeout(TimeSpan.FromMilliseconds(options.RequestTimeout))
            .PingTimeout(TimeSpan.FromMilliseconds(options.PingTimeout))
            .MaximumRetries(options.MaxRetries)
            .DeadTimeout(TimeSpan.FromSeconds(options.DeadTimeout));

        this.client = new ElasticsearchClient(clientSettings);
    }

    public async Task AddEvent(BusinessEvent businessEvent)
    {
        if (await ElasticIsDown()) 
        {
            return;
        }

        var existsResponse = await this.client.Indices.ExistsAsync(ElasticConstants.EVENT_INDEX);

        if (!existsResponse.Exists)
        {
            await this.client.Indices.CreateAsync(ElasticConstants.EVENT_INDEX, cfg =>
            {
                cfg.Mappings(map => map.Properties<BusinessEvent>(p => p
                    .Keyword(k => k.Id!)
                    .Text(t => t.Message)
                    .Text(t => t.Level)
                    .Date(d => d.Timestamp)));
            });
        }

        await this.client.IndexAsync(businessEvent, i => i.Index(ElasticConstants.EVENT_INDEX));
    }

    public async Task AddGeoLocationStat(OrderGeoLoc location)
    {
        if (await ElasticIsDown())
        {
            return;
        }

        var existsResponse = await this.client.Indices.ExistsAsync(ElasticConstants.LOCATION_INDEX);

        if (!existsResponse.Exists)
        {
            await this.client.Indices.CreateAsync(ElasticConstants.LOCATION_INDEX, cfg =>
            {
                cfg.Mappings(map => map.Properties<OrderGeoLoc>(p => p
                    .Keyword(k => k.Id!)
                    .DoubleNumber(t => t.Longitude)
                    .DoubleNumber(t => t.Latitude)
                    .IntegerNumber(n => n.Quantity)
                    .GeoPoint(g => g.GeoLocation)
                    .Date(d => d.Timestamp)));
            });
        }

        await this.client.IndexAsync(location, i => i.Index(ElasticConstants.LOCATION_INDEX));
    }

    private async Task<bool> ElasticIsDown()
    {
        var pingResponse = await this.client.PingAsync(p => p
            .RequestConfiguration(r => r
                .RequestTimeout(TimeSpan.FromMilliseconds(100))));

        var result =  !pingResponse.IsSuccess();

        return result;
    }
}
