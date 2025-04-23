using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;

namespace ButtonShop.Infrastructure.HealthChecks.Checks;

internal sealed class MemoryHealthCheck : IHealthCheck
{
    public static string PATH = "memory_health_check";
    public static string ALOCATED_MEMORY = "allocatedBytes";
    public static string GEN0_COLLECTIONS = "gen0Collections";
    public static string GEN1_COLLECTIONS = "gen1Collections";
    public static string GEN2_COLLECTIONS = "gen2Collections";
    public static string NUMBER_OF_GC_GENERATIONS = "numberOfGenerations";
    public static long MEMORY_THRESHOLD = 1024L * 1024L * 1024L; // 1024 MB TODO move to configuration
    
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var allocated = GC.GetTotalMemory(forceFullCollection: false);
        
        var data = new Dictionary<string, object>()
        {
            { ALOCATED_MEMORY, allocated },
            { GEN0_COLLECTIONS, GC.CollectionCount(0) },
            { GEN1_COLLECTIONS, GC.CollectionCount(1) },
            { GEN2_COLLECTIONS, GC.CollectionCount(2) },
            { NUMBER_OF_GC_GENERATIONS, GC.MaxGeneration },
        };

        var status = (allocated < MEMORY_THRESHOLD) ? HealthStatus.Healthy : HealthStatus.Unhealthy;
        var healthStatus = new HealthCheckResult(status, description: string.Empty, exception: null, data: data);

        return Task.FromResult(healthStatus);
    }
}
