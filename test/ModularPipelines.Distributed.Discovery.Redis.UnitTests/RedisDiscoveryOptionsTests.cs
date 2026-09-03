using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Discovery.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisDiscoveryOptionsTests
{
    [Test]
    public async Task Default_Options_Have_Expected_Values()
    {
        var options = new RedisDiscoveryOptions();

        await Assert.That(options.ConnectionString).IsEqualTo("localhost:6379");
        await Assert.That(options.KeyPrefix).IsEqualTo("modular-pipelines");
        await Assert.That(options.RunIdentifier).IsNull();
        await Assert.That(options.Ttl).IsEqualTo(TimeSpan.FromHours(1));
        await Assert.That(options.DiscoveryTimeout).IsEqualTo(TimeSpan.FromMinutes(2));
        await Assert.That(options.PollInterval).IsEqualTo(TimeSpan.FromMilliseconds(500));
    }

    [Test]
    public async Task Options_Can_Be_Configured()
    {
        var options = new RedisDiscoveryOptions
        {
            ConnectionString = "redis.internal:6380",
            KeyPrefix = "my-pipeline",
            RunIdentifier = "run-123",
            Ttl = TimeSpan.FromHours(2),
            DiscoveryTimeout = TimeSpan.FromMinutes(1),
            PollInterval = TimeSpan.FromMilliseconds(250),
        };

        await Assert.That(options.ConnectionString).IsEqualTo("redis.internal:6380");
        await Assert.That(options.KeyPrefix).IsEqualTo("my-pipeline");
        await Assert.That(options.RunIdentifier).IsEqualTo("run-123");
        await Assert.That(options.Ttl).IsEqualTo(TimeSpan.FromHours(2));
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
                ["Discovery:Ttl"] = "00:30:00",
                ["Discovery:DiscoveryTimeout"] = "00:00:07.500",
                ["Discovery:PollInterval"] = "00:00:00.125",
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
            await Assert.That(options.Ttl).IsEqualTo(TimeSpan.FromMinutes(30));
            await Assert.That(options.DiscoveryTimeout).IsEqualTo(TimeSpan.FromMilliseconds(7500));
            await Assert.That(options.PollInterval).IsEqualTo(TimeSpan.FromMilliseconds(125));
        }
    }

    [Test]
    public async Task HostBuildRejectsIncompleteRestConfiguration()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisSignalRDiscovery(options => options.RestUrl = "https://redis.example");

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(
            () => builder.BuildAsync());

        await Assert.That(exception!.Failures)
            .Contains("RestUrl and RestToken must be configured together.");
    }
}
