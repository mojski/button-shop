using ButtonShop.Application.Events;
using ButtonShop.Common.BusinessMetrics;
using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using StackExchange.Redis;

namespace ButtonShop.Infrastructure.BusinessMonitoring.Metrics;

internal class MetricsAggregationService : IMetricsService
{
    private readonly IDatabase db;
    private readonly InstanceIdentifier instanceIdentifier;
    private readonly BusinessMetricRegistry businessMetricRegistry;

    public MetricsAggregationService(InstanceIdentifier instanceIdentifier, BusinessMetricRegistry businessMetricRegistry, IConnectionMultiplexer multiplexer)
    {
        this.db = multiplexer.GetDatabase();
        this.instanceIdentifier = instanceIdentifier;
        this.businessMetricRegistry = businessMetricRegistry;
    }

    public async Task AddOrderMetrics(OrderAdded order, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();

        foreach (var metric in this.businessMetricRegistry.OrderAddedGaugeResolvers)
        {
            var value = metric.Value(order);

            var key = GetKey(metric.Key);
            var task = this.db.StringIncrementAsync(key, value);
            tasks.Add(task);
        }

        // only small number of tasks
        await Task.WhenAll(tasks);
    }

    public async Task AddShipmentMetrics(CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();

        foreach (var metric in this.businessMetricRegistry.OrderShippedGaugeResolvers)
        {
            var value = metric.Value();

            var key = GetKey(metric.Key);
            var task = this.db.StringIncrementAsync(key, value);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }

    private string GetKey(string metricName)
    {
        var intanceId = this.instanceIdentifier.Id;
        
        return $"{intanceId}_{MetricConstants.PREFIX}_{metricName}";
    }
}
