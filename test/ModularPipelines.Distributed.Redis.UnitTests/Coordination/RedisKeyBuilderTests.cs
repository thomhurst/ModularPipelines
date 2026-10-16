using ModularPipelines.Distributed.Redis.Coordination;

namespace ModularPipelines.Distributed.Redis.UnitTests.Coordination;

public class RedisKeyBuilderTests
{
    [Test]
    public async Task WorkQueue_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.WorkQueue).IsEqualTo("modpipe:{abc123}:work:queue");
    }

    [Test]
    public async Task Results_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.Results).IsEqualTo("modpipe:{abc123}:results");
    }

    [Test]
    public async Task ResultChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.ResultChannel(new ModuleId("MyModule"))).IsEqualTo("modpipe:{abc123}:results:MyModule");
    }

    [Test]
    public async Task Workers_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.Workers).IsEqualTo("modpipe:{abc123}:workers");
    }

    [Test]
    public async Task RunCleanupKeys_Keep_Only_The_Completion_Flag()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        using (Assert.Multiple())
        {
            await Assert.That(builder.RunCleanupKeys).DoesNotContain(builder.CompletionFlag);
            await Assert.That(builder.RunCleanupKeys).Contains(builder.MasterHeartbeat);
            await Assert.That(builder.RunCleanupKeys)
                .IsEquivalentTo(builder.CoordinationKeys.Where(key => key != builder.CompletionFlag).Append(builder.MasterHeartbeat));
            await Assert.That(builder.ArtifactModules).IsEqualTo("modpipe:{abc123}:artifacts:modules");
        }
    }

    [Test]
    public async Task ScriptKeys_ShareRunHashTag()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.WorkQueue).Contains("{abc123}");
        await Assert.That(builder.Workers).Contains("{abc123}");
        await Assert.That(builder.WorkerStatuses).IsEqualTo("modpipe:{abc123}:workers:status");
    }

    [Test]
    public async Task Queue_And_Heartbeat_Keys_Share_The_Run_Hash_Tag()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        using (Assert.Multiple())
        {
            await Assert.That(builder.QueueClasses).IsEqualTo("modpipe:{abc123}:work:classes");
            await Assert.That(builder.QueueIndex).IsEqualTo("modpipe:{abc123}:work:index");
            await Assert.That(builder.WorkerHeartbeats).IsEqualTo("modpipe:{abc123}:workers:heartbeat");
        }
    }

    [Test]
    public async Task ScriptKeys_Follow_The_Order_The_Lua_Scripts_Expect()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.ScriptKeys).IsEquivalentTo(
            [
                builder.WorkQueue,
                builder.QueueClasses,
                builder.QueueIndex,
                builder.Leases,
                builder.Results,
                builder.Workers,
                builder.WorkerHeartbeats,
                builder.WorkerStatuses,
                builder.CompletionFlag,
                builder.CancellationFlag,
                builder.MasterStarted,
                builder.LeaseExpirationWorkers,
                builder.MasterHeartbeat,
            ],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task WorkAvailableChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.WorkAvailableChannel).IsEqualTo("modpipe:{abc123}:work:available");
    }

    [Test]
    public async Task CompletionFlag_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CompletionFlag).IsEqualTo("modpipe:{abc123}:completion");
    }

    [Test]
    public async Task CompletionChannel_ReturnsExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CompletionChannel).IsEqualTo("modpipe:{abc123}:completion:signal");
    }

    [Test]
    public async Task CancellationKeys_ReturnExpectedFormat()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(builder.CancellationFlag).IsEqualTo("modpipe:{abc123}:cancellation");
        await Assert.That(builder.CancellationChannel).IsEqualTo("modpipe:{abc123}:cancellation:signal");
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
    public async Task CoordinationKeys_ContainsAllNonChannelKeys()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        var allKeys = builder.CoordinationKeys.ToList();

        await Assert.That(allKeys).Contains(builder.WorkQueue);
        await Assert.That(allKeys).Contains(builder.QueueClasses);
        await Assert.That(allKeys).Contains(builder.QueueIndex);
        await Assert.That(allKeys).Contains(builder.WorkerHeartbeats);
        await Assert.That(allKeys).Contains(builder.Leases);
        await Assert.That(allKeys).Contains(builder.Results);
        await Assert.That(allKeys).Contains(builder.Workers);
        await Assert.That(allKeys).Contains(builder.WorkerStatuses);
        await Assert.That(allKeys).Contains(builder.CompletionFlag);
        await Assert.That(allKeys).Contains(builder.CancellationFlag);
        await Assert.That(allKeys).Contains(builder.MasterStarted);
    }

    [Test]
    public async Task CoordinationKeys_HasExpectedCount()
    {
        var builder = new RedisKeyBuilder("modpipe", "abc123");

        var allKeys = builder.CoordinationKeys.ToList();

        await Assert.That(allKeys).Count().IsEqualTo(12);
    }
}
