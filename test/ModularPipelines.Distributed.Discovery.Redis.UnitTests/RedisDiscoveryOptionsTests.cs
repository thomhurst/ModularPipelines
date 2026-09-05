using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Discovery.Redis;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RedisDiscoveryOptionsTests
{
    [Test]
    public async Task Default_Options_Have_Expected_Values()
    {
        var options = new RedisDiscoveryOptions();

        await Assert.That(options.ConnectionString).IsEqualTo("localhost:6379");
        await Assert.That(options.KeyPrefix).IsEqualTo("modular-pipelines");
        await Assert.That(options.TtlSeconds).IsEqualTo(3600);
        await Assert.That(options.DiscoveryTimeoutSeconds).IsEqualTo(120);
        await Assert.That(options.PollIntervalMs).IsEqualTo(500);
    }

    [Test]
    public async Task Options_Can_Be_Configured()
    {
        var options = new RedisDiscoveryOptions
        {
            ConnectionString = "redis.internal:6380",
            KeyPrefix = "my-pipeline",
            TtlSeconds = 7200,
            DiscoveryTimeoutSeconds = 60,
            PollIntervalMs = 250,
        };

        await Assert.That(options.ConnectionString).IsEqualTo("redis.internal:6380");
        await Assert.That(options.KeyPrefix).IsEqualTo("my-pipeline");
        await Assert.That(options.TtlSeconds).IsEqualTo(7200);
        await Assert.That(options.DiscoveryTimeoutSeconds).IsEqualTo(60);
        await Assert.That(options.PollIntervalMs).IsEqualTo(250);
    }

    [Test]
    public async Task ConfigurationSectionBindsOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Discovery:ConnectionString"] = "redis.example:6380",
                ["Discovery:KeyPrefix"] = "configured",
            })
            .Build();
        var builder = Pipeline.CreateBuilder();

        builder.AddRedisSignalRDiscovery(configuration.GetSection("Discovery"));
        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(options.ConnectionString).IsEqualTo("redis.example:6380");
            await Assert.That(options.KeyPrefix).IsEqualTo("configured");
        }
    }

    [Test]
    public async Task HostBuildRejectsIncompleteRestConfiguration()
    {
        var builder = Pipeline.CreateBuilder();
        builder.Services.Configure<DistributedOptions>(options => options.RunId = "test-run");
        builder.AddRedisSignalRDiscovery(options => options.RestUrl = "https://redis.example");

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.BuildAsync());

        await Assert.That(exception!.Failures)
            .Contains("RestUrl and RestToken must be configured together.");
    }

    [Test]
    public async Task HostBuildRejectsUnconfiguredRunId()
    {
        var original = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID");
        try
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", null);
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<NoOpModule>();
            builder.AddRedisSignalRDiscovery(_ => { });

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.BuildAsync());

            await Assert.That(exception!.Message).Contains(nameof(DistributedOptions.RunId));
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", original);
        }
    }

    [Test]
    public async Task RunIdCanBeConfiguredAfterDiscoveryRegistration()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisSignalRDiscovery(_ => { });
        builder.AddDistributedMode(options => options.RunId = "configured-after-discovery");

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

        await Assert.That(options.RunId).IsEqualTo("configured-after-discovery");
    }

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
