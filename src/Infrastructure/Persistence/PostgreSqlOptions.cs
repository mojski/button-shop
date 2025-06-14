namespace ButtonShop.Infrastructure.Persistence;

public class PostgreSqlOptions
{
    public const string SECTION_NAME = "PostgreSql";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;

    public string Connection => $"Host={Host};Port={Port};Username={User};Password={Password};Database={Database};";
}
