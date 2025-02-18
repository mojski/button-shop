
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ButtonShop.Infrastructure.OpenTelemetry;

public static class DependencyInjection
{
    internal static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(OpenTelemetryOptions.SECTION_NAME).Get<OpenTelemetryOptions>();

        options ??= new OpenTelemetryOptions();

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ButtonShop", serviceVersion: "1.0"))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(otlpConfig =>
                {
                    if (string.IsNullOrWhiteSpace(options.Authorisation) is false)
                    {
                        otlpConfig.Headers = $"{options.AuthorisationHeader}={options.Authorisation}";
                    }

                    otlpConfig.Endpoint = new Uri(options.Endpoint);
                    otlpConfig.Protocol = options.Protocol;
                }))
            ;
    }

    public static void UseOpenTelemetry(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var options = configuration.GetSection(OpenTelemetryOptions.SECTION_NAME).Get<OpenTelemetryOptions>();

        options ??= new OpenTelemetryOptions();

        loggingBuilder.AddOpenTelemetry(cfg =>
        {
            cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ButtonShop", serviceVersion: "1.0"))
                .AddOtlpExporter(otlpConfig =>
                {
                    if (string.IsNullOrWhiteSpace(options.Authorisation) is false)
                    {
                        otlpConfig.Headers = $"{options.AuthorisationHeader}={options.Authorisation}";
                    }

                    otlpConfig.Endpoint = new Uri(options.Endpoint);
                    otlpConfig.Protocol = options.Protocol;
                })
                ;
        });
    }
}
