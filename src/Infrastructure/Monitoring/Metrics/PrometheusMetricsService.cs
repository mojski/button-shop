namespace ButtonShop.Infrastructure.Monitoring.Metrics;

using ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;
using Prometheus;

public sealed class PrometheusMetricsService : IMetricsService
{
    private readonly Counter blueButtonCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.SOLD_BLUE}", "Total number of blue buttons sold.");
    private readonly Counter greenCounterButtonCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.SOLD_GREEN}", "Total number of green buttons sold.");
    private readonly Counter orderCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.ORDERS_TOTAL}", "Total number of orders.");
    private readonly Counter orderShippedCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.ORDERS_SHIPPED}", "Order shipped.");
    private readonly Gauge orderWaitingGauge = Metrics.CreateGauge($"{MetricConstants.PREFIX}_{MetricConstants.ORDERS_WAITING}", "Order waiting.");

    private readonly Counter redButtonCounter = Metrics.CreateCounter($"{MetricConstants.PREFIX}_{MetricConstants.SOLD_RED}", "Total number of red buttons sold.");

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
