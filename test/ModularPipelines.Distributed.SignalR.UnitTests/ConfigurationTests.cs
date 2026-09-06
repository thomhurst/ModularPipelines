using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.SignalR.Configuration;
using ModularPipelines.Distributed.SignalR.Extensions;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class ConfigurationTests
{
    [Test]
    public async Task Default_Options_Have_Expected_Values()
    {
        var options = new SignalRDistributedOptions();

        await Assert.That(options.MasterUrl).IsEqualTo("http://localhost:5099");
        await Assert.That(options.HubPath).IsEqualTo("/pipeline-hub");
        await Assert.That(options.ConnectionTimeout).IsEqualTo(TimeSpan.FromMinutes(2));
        await Assert.That(options.ReconnectGrace).IsEqualTo(TimeSpan.FromSeconds(45));
        await Assert.That(options.KeepAliveInterval).IsEqualTo(TimeSpan.FromSeconds(5));
        await Assert.That(options.PeerTimeout).IsEqualTo(TimeSpan.FromSeconds(15));
        await Assert.That(options.TunnelStartupTimeout).IsEqualTo(TimeSpan.FromSeconds(30));
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
            ConnectionTimeout = TimeSpan.FromMinutes(1),
            ReconnectGrace = TimeSpan.FromSeconds(30),
            KeepAliveInterval = TimeSpan.FromSeconds(10),
            PeerTimeout = TimeSpan.FromSeconds(25),
            TunnelStartupTimeout = TimeSpan.FromMinutes(1),
            EnableAutoReconnect = false,
            MaxReconnectAttempts = 10,
            MaximumReceiveMessageSize = 2 * 1024 * 1024,
        };

        await Assert.That(options.MasterUrl).IsEqualTo("http://10.0.0.5:8080");
        await Assert.That(options.HubPath).IsEqualTo("/custom-hub");
        await Assert.That(options.ConnectionTimeout).IsEqualTo(TimeSpan.FromMinutes(1));
        await Assert.That(options.ReconnectGrace).IsEqualTo(TimeSpan.FromSeconds(30));
        await Assert.That(options.KeepAliveInterval).IsEqualTo(TimeSpan.FromSeconds(10));
        await Assert.That(options.PeerTimeout).IsEqualTo(TimeSpan.FromSeconds(25));
        await Assert.That(options.TunnelStartupTimeout).IsEqualTo(TimeSpan.FromMinutes(1));
        await Assert.That(options.EnableAutoReconnect).IsFalse();
        await Assert.That(options.MaxReconnectAttempts).IsEqualTo(10);
        await Assert.That(options.MaximumReceiveMessageSize).IsEqualTo(2 * 1024 * 1024);
    }

    [Test]
    public async Task ConfigurationSectionBindsOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SignalR:MasterUrl"] = "https://master.example",
                ["SignalR:HubPath"] = "/distributed",
                ["SignalR:ConnectionTimeout"] = "00:00:07.500",
                ["SignalR:ReconnectGrace"] = "00:00:08.250",
                ["SignalR:KeepAliveInterval"] = "00:00:01.125",
                ["SignalR:PeerTimeout"] = "00:00:03.750",
                ["SignalR:TunnelStartupTimeout"] = "00:00:09.500",
            })
            .Build();
        var builder = Pipeline.CreateBuilder();

        builder.AddSignalRDistributedCoordinator(configuration.GetSection("SignalR"));
        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(options.MasterUrl).IsEqualTo("https://master.example");
            await Assert.That(options.HubPath).IsEqualTo("/distributed");
            await Assert.That(options.ConnectionTimeout).IsEqualTo(TimeSpan.FromMilliseconds(7500));
            await Assert.That(options.ReconnectGrace).IsEqualTo(TimeSpan.FromMilliseconds(8250));
            await Assert.That(options.KeepAliveInterval).IsEqualTo(TimeSpan.FromMilliseconds(1125));
            await Assert.That(options.PeerTimeout).IsEqualTo(TimeSpan.FromMilliseconds(3750));
            await Assert.That(options.TunnelStartupTimeout).IsEqualTo(TimeSpan.FromMilliseconds(9500));
        }
    }
}
