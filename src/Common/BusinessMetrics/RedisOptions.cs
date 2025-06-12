namespace ButtonShop.Common.BusinessMetrics;

public class RedisOptions
{
    public const string SECTION_NAME = "Redis";
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 6379;

    public string Connection => $"{this.Host}:{this.Port}";
}
