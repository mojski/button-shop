namespace ButtonShop.WebApi.Infrastructure.Monitoring.Elastic.Interfaces;

using ButtonShop.WebApi.Infrastructure.Monitoring.Elastic.Models;

public interface IElasticSearchService
{
    Task AddEvent(BusinessEvent businessEvent);
    Task AddGeoLocationStat(OrderGeoLoc location);
}
