using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Discovery.Redis;
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

        await Assert.That(options.ConnectionString).IsEqualTo(string.Empty);
        await Assert.That(options.KeyPrefix).IsEqualTo("modpipe");
        await Assert.That(options.ConfigureConnection).IsNull();
        await Assert.That(options.TimeToLive).IsEqualTo(TimeSpan.FromHours(1));
        await Assert.That(options.DiscoveryTimeout).IsEqualTo(TimeSpan.FromMinutes(2));
        await Assert.That(options.PollInterval).IsEqualTo(TimeSpan.FromMilliseconds(500));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task HostBuildRejectsMissingConnection(bool useConfigurationSection)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        if (useConfigurationSection)
        {
            builder.AddRedisMasterDiscovery(new ConfigurationBuilder().Build().GetSection("Discovery"));
        }
        else
        {
            builder.AddRedisMasterDiscovery(_ => { });
        }
        builder.AddDistributedMode(options => options.RunId = "test-run");

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.Failures.Any(failure => failure.Contains(nameof(RedisDiscoveryOptions.ConnectionString))))
            .IsTrue();
    }

    [Test]
    [Arguments("localhost:6379")]
    [Arguments("redis.internal:6380")]
    public async Task HostBuildAcceptsExplicitConnectionWithoutConnecting(string connectionString)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisMasterDiscovery(options => options.ConnectionString = connectionString);
        builder.AddDistributedMode(options => options.RunId = "test-run");

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IRedisDiscoveryStore>())
            .IsTypeOf<StackExchangeRedisDiscoveryStore>();
    }

    [Test]
    public async Task HostBuildAcceptsRestWithoutTcpConnection()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisMasterDiscovery(options =>
        {
            options.RestUrl = "https://redis.example";
            options.RestToken = "test-token";
        });
        builder.AddDistributedMode(options => options.RunId = "test-run");

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IRedisDiscoveryStore>())
            .IsTypeOf<RestRedisDiscoveryStore>();
        await Assert.That(pipeline.Services.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value.ConnectionString)
            .IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("")]
    [Arguments("redis.internal:6380")]
    public async Task ConnectionCustomizationRunsBeforeConnecting(string connectionString)
    {
        StackExchange.Redis.ConfigurationOptions? capturedConfiguration = null;
        var stopBeforeConnecting = new InvalidOperationException("Stop before network access");
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisMasterDiscovery(options =>
        {
            options.ConnectionString = connectionString;
            options.ConfigureConnection = configuration =>
            {
                capturedConfiguration = configuration;
                if (configuration.EndPoints.Count == 0)
                {
                    configuration.EndPoints.Add("redis.internal", 6380);
                }

                configuration.Ssl = true;
                throw stopBeforeConnecting;
            };
        });
        builder.AddDistributedMode(options => options.RunId = "test-run");
        await using var pipeline = await builder.BuildAsync();
        var store = pipeline.Services.GetRequiredService<IRedisDiscoveryStore>();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => store.GetAsync("test-key", CancellationToken.None));

        await Assert.That(exception).IsSameReferenceAs(stopBeforeConnecting);
        await Assert.That(capturedConfiguration).IsNotNull();
        await Assert.That(capturedConfiguration!.EndPoints.Single())
            .IsEqualTo(new System.Net.DnsEndPoint("redis.internal", 6380));
        await Assert.That(capturedConfiguration.Ssl).IsTrue();
    }

    [Test]
    public async Task Options_Can_Be_Configured()
    {
        var options = new RedisDiscoveryOptions
        {
            ConnectionString = "redis.internal:6380",
            KeyPrefix = "my-pipeline",
            TimeToLive = TimeSpan.FromHours(2),
            DiscoveryTimeout = TimeSpan.FromMinutes(1),
            PollInterval = TimeSpan.FromMilliseconds(250),
        };

        await Assert.That(options.ConnectionString).IsEqualTo("redis.internal:6380");
        await Assert.That(options.KeyPrefix).IsEqualTo("my-pipeline");
        await Assert.That(options.TimeToLive).IsEqualTo(TimeSpan.FromHours(2));
        await Assert.That(options.DiscoveryTimeout).IsEqualTo(TimeSpan.FromMinutes(1));
        await Assert.That(options.PollInterval).IsEqualTo(TimeSpan.FromMilliseconds(250));
    }

    [Test]
    public async Task ConfigurationSectionBindsOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Discovery:ConnectionString"] = "redis.example:6380",
                ["Discovery:KeyPrefix"] = "configured",
                ["Discovery:TimeToLive"] = "00:30:00",
                ["Discovery:DiscoveryTimeout"] = "00:00:07.500",
                ["Discovery:PollInterval"] = "00:00:00.125",
            })
            .Build();
        var builder = Pipeline.CreateBuilder();

        builder.AddRedisMasterDiscovery(configuration.GetSection("Discovery"));
        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(options.ConnectionString).IsEqualTo("redis.example:6380");
            await Assert.That(options.KeyPrefix).IsEqualTo("configured");
            await Assert.That(options.TimeToLive).IsEqualTo(TimeSpan.FromMinutes(30));
            await Assert.That(options.DiscoveryTimeout).IsEqualTo(TimeSpan.FromMilliseconds(7500));
            await Assert.That(options.PollInterval).IsEqualTo(TimeSpan.FromMilliseconds(125));
        }
    }

    [Test]
    public async Task HostBuildRejectsIncompleteRestConfiguration()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(options => options.RestUrl = "https://redis.example");
        builder.Services.Configure<DistributedOptions>(options => options.RunId = "test-run");

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
            builder.AddRedisMasterDiscovery(options => options.ConnectionString = "localhost:6379");

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
        builder.AddRedisMasterDiscovery(options => options.ConnectionString = "localhost:6379");
        builder.AddDistributedMode(options => options.RunId = "configured-after-discovery");

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

        await Assert.That(options.RunId).IsEqualTo("configured-after-discovery");
    }

    [Test]
    public async Task HostBuildRejectsInvalidTimings()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(options =>
        {
            options.ConnectionString = "localhost:6379";
            options.TimeToLive = TimeSpan.Zero;
            options.PollInterval = TimeSpan.Zero;
        });
        builder.Services.Configure<DistributedOptions>(options => options.RunId = "test-run");

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.Failures.Count()).IsEqualTo(2);
    }

    [Test]
    public async Task Discovery_Does_Not_Register_An_Application_Connection()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(options => options.ConfigureConnection = configuration => configuration.Ssl = true);

        await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(StackExchange.Redis.IConnectionMultiplexer)))
            .IsFalse();
    }

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
