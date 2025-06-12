using ButtonShop.BusinessMetricsService.Metrics;
using Prometheus;

namespace ButtonShop.BusinessMetricsService;

internal class BusinessMetricsWorker : BackgroundService
{
    private readonly MetricsCollector collector;
    private readonly ILogger<BusinessMetricsWorker> logger;

    public BusinessMetricsWorker(MetricsCollector collector, ILogger<BusinessMetricsWorker> logger)
    {
        this.collector = collector;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var metricServer = new KestrelMetricServer(port: 9091);

        try
        {
            metricServer.Start();
            this.logger.LogInformation("Metric server started on port 9091.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await this.collector.UpdateMetricsAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
            this.logger.LogWarning("Service canceled by request (Ctrl+C or SIGTERM).");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled exception in BusinessMetricsWorker.");
        }
        finally
        {
            this.logger.LogInformation("Stopping metric server...");
            await metricServer.StopAsync();
            this.logger.LogInformation("Metric server stopped.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Worker stopped.");
        await base.StopAsync(cancellationToken);
    }
}
