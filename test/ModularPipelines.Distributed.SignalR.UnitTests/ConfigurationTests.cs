using ModularPipelines.Distributed.SignalR.Configuration;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class ConfigurationTests
{
    [Test]
    public async Task Default_Options_Have_Expected_Values()
    {
        var options = new SignalRDistributedOptions();

        await Assert.That(options.MasterUrl).IsEqualTo("http://localhost:5099");
        await Assert.That(options.HubPath).IsEqualTo("/pipeline-hub");
        await Assert.That(options.ConnectionTimeoutSeconds).IsEqualTo(30);
        await Assert.That(options.EnableAutoReconnect).IsTrue();
        await Assert.That(options.MaxReconnectAttempts).IsEqualTo(5);
        await Assert.That(options.MaximumReceiveMessageSize).IsEqualTo(1024 * 1024);
    }

    [Test]
    public async Task Options_Can_Be_Configured()
    {
        var options = new SignalRDistributedOptions
        {
            MasterUrl = "http://10.0.0.5:8080",
            HubPath = "/custom-hub",
            ConnectionTimeoutSeconds = 60,
            EnableAutoReconnect = false,
            MaxReconnectAttempts = 10,
            MaximumReceiveMessageSize = 2 * 1024 * 1024,
        };

        await Assert.That(options.MasterUrl).IsEqualTo("http://10.0.0.5:8080");
        await Assert.That(options.HubPath).IsEqualTo("/custom-hub");
        await Assert.That(options.ConnectionTimeoutSeconds).IsEqualTo(60);
        await Assert.That(options.EnableAutoReconnect).IsFalse();
        await Assert.That(options.MaxReconnectAttempts).IsEqualTo(10);
        await Assert.That(options.MaximumReceiveMessageSize).IsEqualTo(2 * 1024 * 1024);
    }
}
