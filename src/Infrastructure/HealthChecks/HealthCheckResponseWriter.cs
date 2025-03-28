using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;

namespace ButtonShop.Infrastructure.HealthChecks;

internal static class HealthCheckResponseWriter
{
    public static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        var options = new JsonSerializerOptions 
        {
            WriteIndented = true, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };

        context.Response.ContentType = MediaTypeNames.Application.Json;

        var data = new
        {
            Status = healthReport.Status.ToString(),
            Duration = healthReport.TotalDuration,
            Info = healthReport.Entries
            .Select(entry => new
            {
                entry.Key,
                entry.Value.Description,
                entry.Value.Duration,
                Status = Enum.GetName(typeof(HealthStatus), entry.Value.Status),
                Error = entry.Value.Exception?.Message,
                entry.Value.Data,
            })
        };

        string json = JsonSerializer.Serialize(data, options);

        return context.Response.WriteAsync(json);
    }
}
