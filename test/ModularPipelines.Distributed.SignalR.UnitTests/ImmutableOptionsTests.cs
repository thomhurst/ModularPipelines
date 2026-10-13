using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class ImmutableOptionsTests
{
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
