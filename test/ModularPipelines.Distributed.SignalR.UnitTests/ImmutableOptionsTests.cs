using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class ImmutableOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task String_Output_Redacts_Credentials_Including_Record_Copies(bool copy)
    {
        var options = new SignalRDistributedOptions
        {
            AccessToken = "private-access-token",
            HubPath = "/diagnostic-hub",
        };
        if (copy)
        {
            options = options with { MaxReconnectAttempts = 2 };
        }

        var output = $"{options}";
        await Assert.That(output).DoesNotContain(options.AccessToken!);
        await Assert.That(output).Contains("AccessToken = <redacted>");
        await Assert.That(output).Contains("HubPath = /diagnostic-hub");
        await Assert.That(output).Contains("AdvertisedUrl = <none>");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task String_Output_Redacts_Endpoint_Urls_Including_Record_Copies(bool copy)
    {
        var options = new SignalRDistributedOptions
        {
            ListenUrl = new Uri("https://private-user:private-password@localhost:5099/listen?token=private-query#private-fragment"),
            AdvertisedUrl = new Uri("https://advertised-user:advertised-password@master.example.com/hub?token=advertised-query#advertised-fragment"),
        };
        if (copy)
        {
            options = options with { MaxReconnectAttempts = 2 };
        }

        var output = $"{options}";
        await Assert.That(output).DoesNotContain("private-");
        await Assert.That(output).DoesNotContain("advertised-");
        await Assert.That(output).Contains("ListenUrl = <redacted>");
        await Assert.That(output).Contains("AdvertisedUrl = <redacted>");
    }

    [Test]
    public async Task Binding_And_Transforms_Apply_In_Registration_Order()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["SignalR:HubPath"] = "/bound-hub",
            ["SignalR:KeyPrefix"] = "bound",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(options => options with { HubPath = "first" });
        builder.AddSignalRDistributedCoordinator(configuration.GetSection("SignalR"));
        builder.AddSignalRDistributedCoordinator(options => options with { HubPath = options.HubPath + "-last" });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value;
        await Assert.That(options.HubPath).IsEqualTo("/bound-hub-last");
    }

    [Test]
    public async Task Null_Transform_Result_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(_ => null!);
        using var provider = builder.Services.BuildServiceProvider();
        await Assert.That(() => provider.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value)
            .Throws<InvalidOperationException>().WithMessageContaining("SignalRDistributedOptions configuration returned null");
    }

    [Test]
    public async Task Nested_Tunnel_Transforms_And_Binding_Preserve_Defaults()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["SignalR:Tunnel:Enabled"] = "true",
            ["SignalR:Tunnel:StartupTimeout"] = "00:00:45",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(configuration.GetSection("SignalR"));
        builder.AddSignalRDistributedCoordinator(options => options with
        {
            Tunnel = options.Tunnel with { CloudflaredPath = "custom-cloudflared" },
        });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value;
        await Assert.That(options.Tunnel.Enabled).IsTrue();
        await Assert.That(options.Tunnel.StartupTimeout).IsEqualTo(TimeSpan.FromSeconds(45));
        await Assert.That(options.Tunnel.CloudflaredPath).IsEqualTo("custom-cloudflared");
    }
}
