using ButtonShop.Application.Events;
using ButtonShop.Common.BusinessMetrics;

namespace ButtonShop.Infrastructure.BusinessMonitoring.Metrics;

internal class BusinessMetricRegistry
{
    public IReadOnlyDictionary<string, Func<OrderAdded, int>> OrderAddedGaugeResolvers { get; }
    public IReadOnlyDictionary<string, Func<int>> OrderShippedGaugeResolvers { get; }

    public BusinessMetricRegistry()
    {
        this.OrderAddedGaugeResolvers = new Dictionary<string, Func<OrderAdded, int>>
        {
            [MetricConstants.SOLD_RED] = (OrderAdded order) => order.Items[Domain.Entities.ButtonColors.Red],
            [MetricConstants.SOLD_GREEN] = (OrderAdded order) => order.Items[Domain.Entities.ButtonColors.Green],
            [MetricConstants.SOLD_BLUE] = (OrderAdded order) => order.Items[Domain.Entities.ButtonColors.Green],
            [MetricConstants.ORDERS_TOTAL] = (OrderAdded order) => 1,
            [MetricConstants.ORDERS_WAITING] = (OrderAdded order) => 1,
        };

        this.OrderShippedGaugeResolvers = new Dictionary<string, Func<int>>
        {
            [MetricConstants.ORDERS_SHIPPED] = () => 1,
            [MetricConstants.ORDERS_WAITING] = () => -1,
        };
    }
}
