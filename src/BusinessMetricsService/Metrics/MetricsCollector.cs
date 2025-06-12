using ButtonShop.BusinessMetricsService.Metrics.Interfaces;
using ButtonShop.Common.BusinessMetrics;
using Prometheus;

namespace ButtonShop.BusinessMetricsService.Metrics;

internal class MetricsCollector
{
    private readonly Dictionary<string, Gauge> gauges = new();
    private readonly IMetricsProvider metricsProvider;
    private readonly ILogger<MetricsCollector> logger;

    public MetricsCollector(IMetricsProvider metricsProvider, ILogger<MetricsCollector> logger)
    {
        this.metricsProvider = metricsProvider;
        this.logger = logger;

        foreach (var gauge in MetricConstants.GAUGES)
        {
            var name = $"{MetricConstants.PREFIX}_{gauge.Key}";
            var gaugeInstance = Prometheus.Metrics.CreateGauge(name, gauge.Value);
            gauges[name] = gaugeInstance;
        }
    }

    public async Task UpdateMetricsAsync(CancellationToken cancellation = default)
    {
        foreach (var kvp in gauges)
        {
            var name = kvp.Key;
            var gauge = kvp.Value;

            try
            {
                var val = await metricsProvider.GetMetric(name, cancellation);
                gauge.Set(val);

                logger.LogDebug("Updated metric {MetricName} = {Value}", name, val);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to update metric {MetricName}", name);
            }
        }
    }
}

