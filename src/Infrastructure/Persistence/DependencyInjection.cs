namespace ButtonShop.Infrastructure.Persistence;

internal static class DependencyInjection
{
    public static void AddMartenDb(this IServiceCollection services, IConfiguration configuration)
    {
        var postgreOptions = configuration.GetSection(PostgreSqlOptions.SECTION_NAME).Get<PostgreSqlOptions>()
                           ?? new PostgreSqlOptions();

        services.AddSingleton(postgreOptions);

        services.AddMarten(opts =>
        {
            opts.Connection(postgreOptions.Connection);

            opts.AutoCreateSchemaObjects = AutoCreate.All;
        });
    }
}
