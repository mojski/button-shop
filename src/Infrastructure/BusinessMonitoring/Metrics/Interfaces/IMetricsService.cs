namespace ButtonShop.Infrastructure.BusinessMonitoring.Metrics.Interfaces;

internal interface IMetricsService
{
    void AddOrder(int value = 1);
    void SellBlue(int count);
    void SellGreen(int count);
    void SellRed(int count);
    void ShipOrders(int value = 1);
}
