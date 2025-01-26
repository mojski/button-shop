namespace ButtonShop.Infrastructure.Monitoring.Metrics.Interfaces;

public interface IMetricsService
{
    void AddOrder(int value = 1);
    void SellBlue(int count);
    void SellGreen(int count);

    void SellRed(int count);
    void ShipOrders(int value = 1);
}
