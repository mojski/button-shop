namespace ButtonShop.Infrastructure.OpenTelemetry;

internal sealed class OpenTelemetryOptions
{
    public static readonly string SECTION_NAME = "OpenTelemetry";
    public string Authorisation { get; init; } = string.Empty;

    public string Host { get; init; } = "localhost";
    public int OpenTelemetryPort { get; init; } = 4317;
    public int HealthCheckPort { get; init; } = 13133;
    public string AuthorisationHeader { get; init; } = "x-otlp-api-key";
    public string Endpoint => $"http://{Host}:{OpenTelemetryPort}";
    public string HealthCheckEndpoint => $"http://{Host}:{HealthCheckPort}/health";
    public OtlpExportProtocol Protocol { get; init; } = OtlpExportProtocol.Grpc;
}
