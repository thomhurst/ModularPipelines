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
        await Assert.That(options.ConnectionTimeoutSeconds).IsEqualTo(120);
        await Assert.That(options.EnableAutoReconnect).IsTrue();
        await Assert.That(options.MaxReconnectAttempts).IsEqualTo(5);
        await Assert.That(options.MaxReceiveMessageSize).IsEqualTo(1024 * 1024);
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
            MaxReceiveMessageSize = 2 * 1024 * 1024,
        };

        await Assert.That(options.MasterUrl).IsEqualTo("http://10.0.0.5:8080");
        await Assert.That(options.HubPath).IsEqualTo("/custom-hub");
        await Assert.That(options.ConnectionTimeoutSeconds).IsEqualTo(60);
        await Assert.That(options.EnableAutoReconnect).IsFalse();
        await Assert.That(options.MaxReconnectAttempts).IsEqualTo(10);
        await Assert.That(options.MaxReceiveMessageSize).IsEqualTo(2 * 1024 * 1024);
    }

    [Test]
    public async Task ConfigurationSectionBindsOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SignalR:MasterUrl"] = "https://master.example",
                ["SignalR:HubPath"] = "/distributed",
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
        }
    }
}
