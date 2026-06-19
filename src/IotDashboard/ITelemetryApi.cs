namespace IotDashboard;

/// <summary>
/// Local abstraction over the upstream device telemetry API. Implementations return a
/// <see cref="TelemetryResponse"/> describing the outcome of a status lookup.
/// </summary>
public interface ITelemetryApi
{
    Task<TelemetryResponse> GetDeviceStatusAsync(string deviceId);
}
