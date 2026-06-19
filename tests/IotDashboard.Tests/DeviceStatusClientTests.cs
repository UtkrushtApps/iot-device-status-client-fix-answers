using IotDashboard;
using Xunit;

namespace IotDashboard.Tests;

public class DeviceStatusClientTests
{
    private static DeviceStatusClient CreateClient(FakeTelemetryApi api) => new(api);

    [Fact]
    public async Task HealthyDevice_ReturnsPopulatedStatus()
    {
        var api = new FakeTelemetryApi();
        var client = CreateClient(api);

        DeviceStatus? result = await client.GetStatusAsync("healthy-1");

        Assert.NotNull(result);
        Assert.Equal("healthy-1", result!.DeviceId);
        Assert.True(result.IsOnline);
        Assert.Equal(82, result.BatteryPercent);
    }

    [Fact]
    public async Task MissingDevice_ReturnsNull()
    {
        var api = new FakeTelemetryApi();
        var client = CreateClient(api);

        DeviceStatus? result = await client.GetStatusAsync("missing-1");

        Assert.Null(result);
    }

    [Fact]
    public async Task ServerError_ThrowsInvalidOperation()
    {
        var api = new FakeTelemetryApi();
        var client = CreateClient(api);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.GetStatusAsync("server-error-1"));

        Assert.Equal("Device status request failed", ex.Message);
    }

    [Fact]
    public async Task MalformedPayload_ThrowsInvalidOperationWithClearMessage()
    {
        var api = new FakeTelemetryApi();
        var client = CreateClient(api);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.GetStatusAsync("malformed-1"));

        Assert.Equal("Device status payload was malformed", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task BlankDeviceId_ThrowsArgumentException(string deviceId)
    {
        var api = new FakeTelemetryApi();
        var client = CreateClient(api);

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.GetStatusAsync(deviceId));
    }

    [Fact]
    public async Task BlankDeviceId_DoesNotCallUpstream()
    {
        var api = new FakeTelemetryApi();
        var client = CreateClient(api);

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.GetStatusAsync("   "));

        Assert.False(api.WasCalled);
    }
}
