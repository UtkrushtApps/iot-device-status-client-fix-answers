using System.Text.Json.Serialization;

namespace IotDashboard;

/// <summary>
/// Shape of the JSON payload returned by the upstream telemetry API.
/// </summary>
public sealed class DeviceStatusPayload
{
    [JsonPropertyName("deviceId")]
    public string? DeviceId { get; set; }

    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; set; }

    [JsonPropertyName("batteryPercent")]
    public int? BatteryPercent { get; set; }
}
