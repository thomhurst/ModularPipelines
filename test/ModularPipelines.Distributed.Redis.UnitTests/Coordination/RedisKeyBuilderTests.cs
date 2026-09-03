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
    public async Task WorkerHeartbeat_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.WorkerHeartbeatPrefix)
            .IsEqualTo("modpipe:abc123:workers:");
        await Assert.That(builder.WorkerHeartbeat(7))
            .IsEqualTo("modpipe:abc123:workers:7:heartbeat");
    }

    [Test]
    public async Task WorkAvailableChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.WorkAvailableChannel).IsEqualTo("modpipe:abc123:work:available");
    }

    [Test]
    public async Task CompletionFlag_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CompletionFlag).IsEqualTo("modpipe:abc123:completion");
    }

    [Test]
    public async Task CompletionChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CompletionChannel).IsEqualTo("modpipe:abc123:completion:signal");
    }

    [Test]
    public async Task CancellationKeys_ReturnExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CancellationFlag).IsEqualTo("modpipe:abc123:cancellation");
        await Assert.That(builder.CancellationChannel).IsEqualTo("modpipe:abc123:cancellation:signal");
    }

    [Test]
    public async Task CustomPrefix_UsedInAllKeys()
    {
        var builder = new RedisKeyBuilder("custom", "run-42");

        await Assert.That(builder.WorkQueue).StartsWith("custom:");
        await Assert.That(builder.Results).StartsWith("custom:");
        await Assert.That(builder.Workers).StartsWith("custom:");
    }

    [Test]
    public async Task AllStorageKeys_ContainsAllNonChannelKeys()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        var allKeys = builder.AllStorageKeys.ToList();

        await Assert.That(allKeys).Contains(builder.WorkQueue);
        await Assert.That(allKeys).Contains(builder.Results);
        await Assert.That(allKeys).Contains(builder.Workers);
        await Assert.That(allKeys).Contains(builder.CompletionFlag);
        await Assert.That(allKeys).Contains(builder.CancellationFlag);
    }

    [Test]
    public async Task AllStorageKeys_HasExpectedCount()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        var allKeys = builder.AllStorageKeys.ToList();

        await Assert.That(allKeys).Count().IsEqualTo(5);
    }
}
