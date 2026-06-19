# Solution Steps

1. Open `src/IotDashboard/DeviceStatusClient.cs` and add early input validation at the start of `GetStatusAsync` using `string.IsNullOrWhiteSpace(deviceId)`. Throw `ArgumentException` before calling `_telemetryApi.GetDeviceStatusAsync`.

2. Call `_telemetryApi.GetDeviceStatusAsync(deviceId)` only after validation succeeds.

3. Inspect `TelemetryResponse.StatusCode`: return `null` for `404`, and throw `InvalidOperationException` with the exact message `Device status request failed` for any status code outside the success range `200` through `299`.

4. For successful responses, deserialize `TelemetryResponse.Body` with `JsonSerializer.Deserialize<DeviceStatusPayload>`, but first reject null, empty, or whitespace bodies as malformed.

5. Wrap JSON deserialization failures in `InvalidOperationException` with the exact message `Device status payload was malformed`.

6. After deserialization, verify the payload exists and that `batteryPercent` was present by checking `payload.BatteryPercent is not null`. Treat a missing device id as malformed as well to avoid returning an invalid `DeviceStatus`.

7. Return a new `DeviceStatus` populated from the payload’s `DeviceId`, `IsOnline`, and `BatteryPercent.Value`.

8. Run `dotnet test` from `/root/task` and confirm all tests pass.

