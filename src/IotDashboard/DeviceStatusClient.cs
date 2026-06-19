using System.Text.Json;

namespace IotDashboard;

/// <summary>
/// Reads device status from the upstream telemetry API for the dashboard.
/// </summary>
public sealed class DeviceStatusClient
{
    private const string RequestFailedMessage = "Device status request failed";
    private const string MalformedPayloadMessage = "Device status payload was malformed";

    private readonly ITelemetryApi _telemetryApi;

    public DeviceStatusClient(ITelemetryApi telemetryApi)
    {
        _telemetryApi = telemetryApi ?? throw new ArgumentNullException(nameof(telemetryApi));
    }

    public async Task<DeviceStatus?> GetStatusAsync(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("Device id must not be null, empty, or whitespace.", nameof(deviceId));
        }

        TelemetryResponse response = await _telemetryApi.GetDeviceStatusAsync(deviceId);

        if (response.StatusCode == 404)
        {
            return null;
        }

        if (response.StatusCode < 200 || response.StatusCode >= 300)
        {
            throw new InvalidOperationException(RequestFailedMessage);
        }

        DeviceStatusPayload payload = DeserializePayload(response.Body);

        if (string.IsNullOrWhiteSpace(payload.DeviceId) || payload.BatteryPercent is null)
        {
            throw new InvalidOperationException(MalformedPayloadMessage);
        }

        return new DeviceStatus(
            payload.DeviceId,
            payload.IsOnline,
            payload.BatteryPercent.Value);
    }

    private static DeviceStatusPayload DeserializePayload(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new InvalidOperationException(MalformedPayloadMessage);
        }

        try
        {
            DeviceStatusPayload? payload = JsonSerializer.Deserialize<DeviceStatusPayload>(body);

            return payload ?? throw new InvalidOperationException(MalformedPayloadMessage);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(MalformedPayloadMessage, ex);
        }
    }
}
