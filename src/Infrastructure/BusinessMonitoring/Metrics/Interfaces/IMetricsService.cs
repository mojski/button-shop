using ButtonShop.Application.Events;

namespace ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;

internal interface IMetricsService
{
    Task AddOrderMetrics(OrderAdded order, CancellationToken cancellationToken = default);
    Task AddShipmentMetrics(CancellationToken cancellationToken = default);
}
