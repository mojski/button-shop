namespace ButtonShop.Infrastructure.OpenTelemetry;

internal sealed class OpenTelemetryOptions
{
    public static readonly string SECTION_NAME = "OpenTelemetry";
    public string Authorisation { get; init; } = string.Empty;
    public string AuthorisationHeader { get; init; } = "x-otlp-api-key";

    public string Endpoint { get; init; } = "http://localhost:4317";
    public string HealthCheckEndpoint { get; init; } = "http://localhost:13133/health";

    public OtlpExportProtocol Protocol { get; init; } = OtlpExportProtocol.Grpc;
}
