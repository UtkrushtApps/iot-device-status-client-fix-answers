namespace IotDashboard;

/// <summary>
/// Status of a single device as shown on the dashboard.
/// </summary>
public sealed record DeviceStatus(string DeviceId, bool IsOnline, int BatteryPercent);
