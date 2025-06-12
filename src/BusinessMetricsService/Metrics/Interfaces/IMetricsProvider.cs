namespace ButtonShop.BusinessMetricsService.Metrics.Interfaces;

internal interface IMetricsProvider
{
    Task<int> GetMetric(string pattern, CancellationToken cancellationToken = default);
}
