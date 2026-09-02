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
}
