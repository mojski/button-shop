using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.Infrastructure.Monitoring.Elastic.Models;

namespace ButtonShop.IntegrationTests.Fakes;

internal class FakeElasticSearchService : IElasticSearchService
{
    public Task AddEvent(BusinessEvent businessEvent) => Task.CompletedTask;

    public Task AddGeoLocationStat(OrderGeoLoc location) => Task.CompletedTask;
}
