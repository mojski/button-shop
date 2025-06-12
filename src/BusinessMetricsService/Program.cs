using ButtonShop.BusinessMetricsService;
using ButtonShop.BusinessMetricsService.Metrics;

var tokenSource = new CancellationTokenSource();
var cancellationToken = tokenSource.Token;

Console.CancelKeyPress += (_, ea) =>
{
    ea.Cancel = true;
    tokenSource.Cancel();
};

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddBusinessMetricCollection(configuration);
        services.AddHostedService<BusinessMetricsWorker>();
        
    });

var host = builder.Build();

await host.RunAsync(cancellationToken);
