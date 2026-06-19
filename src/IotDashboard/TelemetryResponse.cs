namespace IotDashboard;

/// <summary>
/// Represents a response from the upstream telemetry API. The <see cref="StatusCode"/>
/// mirrors an HTTP status code, and <see cref="Body"/> contains the JSON payload when present.
/// </summary>
public sealed class TelemetryResponse
{
    public int StatusCode { get; init; }

    public string? Body { get; init; }
}
