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
            RunIdentifier = "run-123",
            TtlSeconds = 7200,
            DiscoveryTimeoutSeconds = 60,
            PollIntervalMs = 250,
        };

        await Assert.That(options.ConnectionString).IsEqualTo("redis.internal:6380");
        await Assert.That(options.KeyPrefix).IsEqualTo("my-pipeline");
        await Assert.That(options.RunIdentifier).IsEqualTo("run-123");
        await Assert.That(options.TtlSeconds).IsEqualTo(7200);
        await Assert.That(options.DiscoveryTimeoutSeconds).IsEqualTo(60);
        await Assert.That(options.PollIntervalMs).IsEqualTo(250);
    }
}
