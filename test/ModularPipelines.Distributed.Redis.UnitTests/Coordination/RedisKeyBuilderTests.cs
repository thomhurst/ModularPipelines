using ModularPipelines.Distributed.Redis.Coordination;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisKeyBuilderTests
{
    [Test]
    public async Task WorkQueue_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.WorkQueue).IsEqualTo("modpipe:abc123:work:queue");
    }

    [Test]
    public async Task Results_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.Results).IsEqualTo("modpipe:abc123:results");
    }

    [Test]
    public async Task ResultChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.ResultChannel("MyModule")).IsEqualTo("modpipe:abc123:results:MyModule");
    }

    [Test]
    public async Task Workers_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.Workers).IsEqualTo("modpipe:abc123:workers");
    }

    [Test]
    public async Task Heartbeats_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.Heartbeats).IsEqualTo("modpipe:abc123:heartbeats");
    }

    [Test]
    public async Task Cancellation_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.Cancellation).IsEqualTo("modpipe:abc123:cancellation");
    }

    [Test]
    public async Task CancellationChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CancellationChannel).IsEqualTo("modpipe:abc123:cancellation:signal");
    }

    [Test]
    public async Task CustomPrefix_UsedInAllKeys()
    {
        var builder = new RedisKeyBuilder("custom", "run-42");

        await Assert.That(builder.WorkQueue).StartsWith("custom:");
        await Assert.That(builder.Results).StartsWith("custom:");
        await Assert.That(builder.Workers).StartsWith("custom:");
        await Assert.That(builder.Heartbeats).StartsWith("custom:");
        await Assert.That(builder.Cancellation).StartsWith("custom:");
        await Assert.That(builder.CancellationChannel).StartsWith("custom:");
    }

    [Test]
    public async Task AllStorageKeys_ContainsAllNonChannelKeys()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        var allKeys = builder.AllStorageKeys.ToList();

        await Assert.That(allKeys).Contains(builder.WorkQueue);
        await Assert.That(allKeys).Contains(builder.Results);
        await Assert.That(allKeys).Contains(builder.Workers);
        await Assert.That(allKeys).Contains(builder.Heartbeats);
        await Assert.That(allKeys).Contains(builder.Cancellation);
    }

    [Test]
    public async Task AllStorageKeys_HasExpectedCount()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        var allKeys = builder.AllStorageKeys.ToList();

        await Assert.That(allKeys).Count().IsEqualTo(5);
    }
}
