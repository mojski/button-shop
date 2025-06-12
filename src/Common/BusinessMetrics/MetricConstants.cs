namespace ButtonShop.Common.BusinessMetrics;

public static class MetricConstants
{

    public const string PREFIX = "button_shop";
    public const string ORDERS_SHIPPED = "orders_shipped";
    public const string ORDERS_TOTAL = "orders_total";
    public const string ORDERS_WAITING = "orders_waiting";
    public const string SOLD_BLUE = "sold_blue";
    public const string SOLD_GREEN = "sold_green";
    public const string SOLD_RED = "sold_red";
    public const string ORDERS_SHIPPED_TITLE = "Orders shipped.";
    public const string ORDERS_TOTAL_TITLE = "Orders Total";
    public const string ORDERS_WAITING_TITLE = "Orders waiting";
    public const string SOLD_BLUE_TITLE = "Total number of blue buttons sold.";
    public const string SOLD_GREEN_TITLE = "Total number of green buttons sold.";
    public const string SOLD_RED_TITLE = "Total number of red buttons sold.";

    public static IReadOnlyDictionary<string, string> COUNTERS =>
    new Dictionary<string, string> {};

    public static IReadOnlyDictionary<string, string> GAUGES =>
    new Dictionary<string, string>
    {
        { ORDERS_WAITING, ORDERS_WAITING_TITLE },
        { SOLD_RED, SOLD_RED_TITLE },
        { SOLD_GREEN, SOLD_GREEN_TITLE },
        { SOLD_BLUE, SOLD_BLUE_TITLE },
        { ORDERS_TOTAL, ORDERS_TOTAL_TITLE },
        { ORDERS_SHIPPED, ORDERS_SHIPPED_TITLE },
    };
}
