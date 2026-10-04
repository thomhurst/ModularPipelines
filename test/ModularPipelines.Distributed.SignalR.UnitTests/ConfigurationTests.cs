using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.SignalR.Coordination;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class ConfigurationTests
{
    [Test]
    public async Task Default_Options_Have_Expected_Values()
    {
        var options = new SignalRDistributedOptions();

        using (Assert.Multiple())
        {
            await Assert.That(options.ListenUrl).IsEqualTo(new Uri("http://localhost:5099"));
            await Assert.That(options.AdvertisedUrl).IsNull();
            await Assert.That(options.AccessToken).IsNull();
            await Assert.That(options.HubPath).IsEqualTo("/pipeline-hub");
            await Assert.That(options.ConnectionTimeout).IsEqualTo(TimeSpan.FromMinutes(2));
            await Assert.That(options.KeepAliveInterval).IsEqualTo(TimeSpan.FromSeconds(5));
            await Assert.That(options.PeerTimeout).IsEqualTo(TimeSpan.FromSeconds(15));
            await Assert.That(options.MaxReconnectAttempts).IsEqualTo(5);
            await Assert.That(options.MaxMessageSizeBytes).IsEqualTo(1024 * 1024);
            await Assert.That(options.Tunnel.Enabled).IsFalse();
            await Assert.That(options.Tunnel.CloudflaredPath).IsEqualTo("cloudflared");
            await Assert.That(options.Tunnel.StartupTimeout).IsEqualTo(TimeSpan.FromSeconds(30));
        }
    }

    [Test]
    public async Task ConfigurationSectionBindsOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SignalR:ListenUrl"] = "http://0.0.0.0:6000",
                ["SignalR:AdvertisedUrl"] = "https://master.example",
                ["SignalR:HubPath"] = "/distributed",
                ["SignalR:AccessToken"] = "token",
                ["SignalR:ConnectionTimeout"] = "00:00:07.500",
                ["SignalR:KeepAliveInterval"] = "00:00:01.125",
                ["SignalR:PeerTimeout"] = "00:00:03.750",
                ["SignalR:MaxMessageSizeBytes"] = "2048",
                ["SignalR:Tunnel:Enabled"] = "true",
                ["SignalR:Tunnel:StartupTimeout"] = "00:00:09.500",
            })
            .Build();
        var builder = Pipeline.CreateBuilder();

        builder.AddSignalRDistributedCoordinator(configuration.GetSection("SignalR"));
        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(options.ListenUrl).IsEqualTo(new Uri("http://0.0.0.0:6000"));
            await Assert.That(options.AdvertisedUrl).IsEqualTo(new Uri("https://master.example"));
            await Assert.That(options.HubPath).IsEqualTo("/distributed");
            await Assert.That(options.AccessToken).IsEqualTo("token");
            await Assert.That(options.ConnectionTimeout).IsEqualTo(TimeSpan.FromMilliseconds(7500));
            await Assert.That(options.KeepAliveInterval).IsEqualTo(TimeSpan.FromMilliseconds(1125));
            await Assert.That(options.PeerTimeout).IsEqualTo(TimeSpan.FromMilliseconds(3750));
            await Assert.That(options.MaxMessageSizeBytes).IsEqualTo(2048);
            await Assert.That(options.Tunnel.Enabled).IsTrue();
            await Assert.That(options.Tunnel.StartupTimeout).IsEqualTo(TimeSpan.FromMilliseconds(9500));
        }
    }

    [Test]
    public async Task Invalid_Options_Fail_Validation()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(options =>
        {
            options.ListenUrl = new Uri("not a url", UriKind.Relative);
            options.HubPath = "hub";
            options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            options.PeerTimeout = TimeSpan.FromSeconds(5);
            options.MaxReconnectAttempts = -1;
        });
        using var services = builder.Services.BuildServiceProvider();

        var exception = await Assert.That(() => services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value)
            .Throws<OptionsValidationException>();

        await Assert.That(exception!.Failures.Count()).IsEqualTo(4);
    }

    [Test]
    public async Task Registration_Requires_An_Explicit_Run_Id()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(_ => { });

        await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(ModularPipelines.Distributed.Configuration.ExplicitRunIdRequirement)))
            .IsTrue();
    }

    [Test]
    public async Task Loopback_Master_Needs_No_Token()
    {
        var token = SignalRDistributedCoordinatorFactory.ResolveMasterAccessToken(
            new SignalRDistributedOptions { ListenUrl = new Uri("http://127.0.0.1:0") },
            hasDiscovery: false);

        await Assert.That(token).IsNull();
    }

    [Test]
    public async Task Reachable_Master_Generates_Token_For_Discovery()
    {
        var first = SignalRDistributedCoordinatorFactory.ResolveMasterAccessToken(
            new SignalRDistributedOptions { ListenUrl = new Uri("http://0.0.0.0:5099") },
            hasDiscovery: true);
        var second = SignalRDistributedCoordinatorFactory.ResolveMasterAccessToken(
            new SignalRDistributedOptions { ListenUrl = new Uri("http://localhost:5099"), Tunnel = { Enabled = true } },
            hasDiscovery: true);

        await Assert.That(first!.Length).IsGreaterThanOrEqualTo(40);
        await Assert.That(second).IsNotNull().And.IsNotEqualTo(first);
    }

    [Test]
    public async Task Reachable_Master_Without_Token_Or_Discovery_Fails()
    {
        await Assert.That(() => SignalRDistributedCoordinatorFactory.ResolveMasterAccessToken(
                new SignalRDistributedOptions { ListenUrl = new Uri("http://0.0.0.0:5099") },
                hasDiscovery: false))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Loopback_Master_With_Remote_Advertised_Url_Needs_Token()
    {
        await Assert.That(() => SignalRDistributedCoordinatorFactory.ResolveMasterAccessToken(
                new SignalRDistributedOptions
                {
                    ListenUrl = new Uri("http://127.0.0.1:5099"),
                    AdvertisedUrl = new Uri("https://pipelines.example.com"),
                },
                hasDiscovery: false))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Configured_Token_Is_Used()
    {
        var token = SignalRDistributedCoordinatorFactory.ResolveMasterAccessToken(
            new SignalRDistributedOptions { ListenUrl = new Uri("http://0.0.0.0:5099"), AccessToken = "configured" },
            hasDiscovery: false);

        await Assert.That(token).IsEqualTo("configured");
    }

    [Test]
    public async Task Second_Coordinator_Backend_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedMode(options => options.RunId = "run");
        builder.AddSignalRDistributedCoordinator(_ => { });
        builder.AddDistributedCoordinator<ModularPipelines.Distributed.Coordination.InMemoryDistributedCoordinator>();

        await Assert.That(async () => await builder.BuildAsync()).Throws<InvalidOperationException>();
    }
}
