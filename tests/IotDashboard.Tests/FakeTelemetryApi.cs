using IotDashboard;

namespace IotDashboard.Tests;

/// <summary>
/// In-memory telemetry API used to simulate upstream responses deterministically.
/// </summary>
public sealed class FakeTelemetryApi : ITelemetryApi
{
    public bool WasCalled { get; private set; }

    public Task<TelemetryResponse> GetDeviceStatusAsync(string deviceId)
    {
        WasCalled = true;

        return deviceId switch
        {
            "healthy-1" => Task.FromResult(new TelemetryResponse
            {
                StatusCode = 200,
                Body = "{\"deviceId\":\"healthy-1\",\"isOnline\":true,\"batteryPercent\":82}"
            }),
            "missing-1" => Task.FromResult(new TelemetryResponse
            {
                StatusCode = 404,
                Body = null
            }),
            "server-error-1" => Task.FromResult(new TelemetryResponse
            {
                StatusCode = 500,
                Body = null
            }),
            "malformed-1" => Task.FromResult(new TelemetryResponse
            {
                StatusCode = 200,
                Body = "{\"deviceId\":\"malformed-1\",\"isOnline\":true}"
            }),
            _ => Task.FromResult(new TelemetryResponse
            {
                StatusCode = 404,
                Body = null
            })
        };
    }
}
