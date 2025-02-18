namespace ButtonShop.Infrastructure.BusinessMonitoring.Metrics;

using ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;
using Prometheus;

internal sealed class PrometheusMetricsService : IMetricsService
{
    private readonly Counter redButtonCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.SOLD_RED}", MetricConstants.SOLD_RED_TITLE);
    private readonly Counter greenCounterButtonCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.SOLD_GREEN}", MetricConstants.SOLD_GREEN_TITLE);
    private readonly Counter blueButtonCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.SOLD_BLUE}", MetricConstants.SOLD_BLUE_TITLE);
    private readonly Counter orderCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.ORDERS_TOTAL}", MetricConstants.ORDERS_TOTAL_TITLE);
    private readonly Counter orderShippedCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.ORDERS_SHIPPED}", MetricConstants.ORDERS_SHIPPED_TITLE);
    private readonly Gauge orderWaitingGauge = Metrics.CreateGauge($"{MetricConstants.PREFIX}_{MetricConstants.ORDERS_WAITING}", MetricConstants.ORDERS_WAITING_TITLE);

    public void AddOrder(int value = 1)
    {
        this.orderCounter.Inc(value);
        this.orderWaitingGauge.Inc(value);
    }

    public void SellBlue(int count) => this.blueButtonCounter.Inc(count);

    public void SellGreen(int count) => this.greenCounterButtonCounter.Inc(count);

    public void SellRed(int count) => this.redButtonCounter.Inc(count);

    public void ShipOrders(int value = 1)
    {
        this.orderShippedCounter.Inc(value);
        this.orderWaitingGauge.Dec(value);
    }
}
