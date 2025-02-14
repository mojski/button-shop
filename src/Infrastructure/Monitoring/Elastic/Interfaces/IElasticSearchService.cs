namespace ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;

using ButtonShop.Infrastructure.Monitoring.Elastic.Models;

internal interface IElasticSearchService
{
    Task AddEvent(BusinessEvent businessEvent);
    Task AddGeoLocationStat(OrderGeoLoc location);
}
