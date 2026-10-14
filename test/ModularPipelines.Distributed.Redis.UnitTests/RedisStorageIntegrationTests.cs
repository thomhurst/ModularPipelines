using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Distributed.Redis.Coordination;
using Respire;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.Redis.UnitTests;

/// <summary>
/// Runs the artifact store and module cache against a real Redis server when
/// <c>MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING</c> is set.
/// </summary>
public class RedisStorageIntegrationTests
{
    private const string ConnectionStringVariable = "MODULAR_PIPELINES_REDIS_TEST_CONNECTION_STRING";
    private const string Fingerprint = "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB";

    [Test]
    [Arguments(0)]
    [Arguments(7)]
    [Arguments(16)]
    [Arguments(40)]
    public async Task Artifact_RoundTrips(int length)
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions
        {
            KeyPrefix = "modpipe-integration",
            ChunkSizeBytes = 16,
            TimeToLive = TimeSpan.FromMinutes(1),
        };
        var store = new RedisDistributedArtifactStore(
            connection,
            new RedisKeyBuilder(options.KeyPrefix, Guid.NewGuid().ToString("N")),
            options);
        var data = Enumerable.Range(0, length).Select(value => (byte) value).ToArray();

        using var source = new MemoryStream(data);
        var reference = await store.UploadAsync(new ArtifactDescriptor { Name = "artifact", ModuleId = "Integration.Module" }, source, CancellationToken.None);
        await using var downloaded = await store.DownloadAsync(reference, CancellationToken.None);
        using var copy = new MemoryStream();
        await downloaded.CopyToAsync(copy);
        var listed = await store.ListArtifactsAsync("Integration.Module", CancellationToken.None);
        await store.DeleteAsync(reference, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(reference.SizeBytes).IsEqualTo(length);
            await Assert.That(copy.ToArray()).IsEquivalentTo(data);
            await Assert.That(listed.Select(item => item.ArtifactId)).Contains(reference.ArtifactId);
        }
    }

    [Test]
    public async Task Run_Cleanup_Deletes_Artifacts_Uploaded_By_Every_Store_In_The_Run()
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions
        {
            KeyPrefix = "modpipe-integration",
            ChunkSizeBytes = 16,
            TimeToLive = TimeSpan.FromMinutes(1),
        };
        var runId = Guid.NewGuid().ToString("N");
        var keys = new RedisKeyBuilder(options.KeyPrefix, runId);

        // Each process has its own store; the master's store cleans up artifacts a worker uploaded.
        var workerStore = new RedisDistributedArtifactStore(connection, keys, options);
        var masterStore = new RedisDistributedArtifactStore(connection, keys, options);
        var otherRunStore = new RedisDistributedArtifactStore(
            connection,
            new RedisKeyBuilder(options.KeyPrefix, Guid.NewGuid().ToString("N")),
            options);
        using var small = new MemoryStream([1, 2, 3]);
        using var chunked = new MemoryStream(new byte[40]);
        using var otherRunData = new MemoryStream([4]);
        var smallReference = await workerStore.UploadAsync(
            new ArtifactDescriptor { Name = "small", ModuleId = "Integration.Worker" }, small, CancellationToken.None);
        var chunkedReference = await workerStore.UploadAsync(
            new ArtifactDescriptor { Name = "chunked", ModuleId = "Integration.Other" }, chunked, CancellationToken.None);
        await otherRunStore.UploadAsync(
            new ArtifactDescriptor { Name = "kept", ModuleId = "Integration.Worker" }, otherRunData, CancellationToken.None);

        await masterStore.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = runId, PipelineSucceeded = true },
            CancellationToken.None);

        RespireKey[] deletedKeys =
        [
            keys.ArtifactMeta(smallReference.ArtifactId),
            keys.ArtifactData(smallReference.ArtifactId),
            keys.ArtifactMeta(chunkedReference.ArtifactId),
            keys.ArtifactChunk(chunkedReference.ArtifactId, 0),
            keys.ArtifactChunk(chunkedReference.ArtifactId, 2),
            keys.ArtifactIndex("Integration.Worker"),
            keys.ArtifactIndex("Integration.Other"),
            keys.ArtifactModules,
        ];
        using (Assert.Multiple())
        {
            foreach (var key in deletedKeys)
            {
                await Assert.That(await connection.ExistsAsync(key)).IsFalse();
            }

            await Assert.That(await otherRunStore.ListArtifactsAsync("Integration.Worker", CancellationToken.None))
                .HasSingleItem();
        }
    }

    [Test]
    public async Task Run_Cleanup_Deletes_The_Chunks_Of_A_Failed_Upload()
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions
        {
            KeyPrefix = "modpipe-integration",
            ChunkSizeBytes = 16,
            TimeToLive = TimeSpan.FromMinutes(1),
        };
        var keys = new RedisKeyBuilder(options.KeyPrefix, Guid.NewGuid().ToString("N"));
        var store = new RedisDistributedArtifactStore(connection, keys, options, retryBaseDelay: TimeSpan.Zero);

        await using var source = new FailAfterStream(new byte[16]);
        await Assert.That(async () => await store.UploadAsync(
                new ArtifactDescriptor { Name = "partial", ModuleId = "Integration.Partial" },
                source,
                CancellationToken.None))
            .Throws<IOException>();
        var artifactId = (await connection.Sets.MembersAsync(keys.ArtifactIndex("Integration.Partial"))).Single();
        await Assert.That(await connection.ExistsAsync(keys.ArtifactChunk(artifactId, 0))).IsTrue();
        await Assert.That((await connection.Keys.ExpiryAsync(keys.ArtifactIndex("Integration.Partial"))).HasExpiry).IsTrue();
        await Assert.That((await connection.Keys.ExpiryAsync(keys.ArtifactModules)).HasExpiry).IsTrue();

        await store.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = "partial", PipelineSucceeded = false },
            CancellationToken.None);

        await Assert.That(await connection.ExistsAsync(keys.ArtifactChunk(artifactId, 0))).IsFalse();
    }

    [Test]
    public async Task Run_Cleanup_Deletes_Every_Chunk_When_Early_Chunks_Were_Evicted()
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions
        {
            KeyPrefix = "modpipe-integration",
            ChunkSizeBytes = 16,
            TimeToLive = TimeSpan.FromMinutes(1),
        };
        var keys = new RedisKeyBuilder(options.KeyPrefix, Guid.NewGuid().ToString("N"));
        var store = new RedisDistributedArtifactStore(connection, keys, options);

        // 300 chunks, more than one delete batch.
        using var source = new MemoryStream(new byte[16 * 300]);
        var reference = await store.UploadAsync(
            new ArtifactDescriptor { Name = "large", ModuleId = "Integration.Large" }, source, CancellationToken.None);
        await connection.DeleteAsync([keys.ArtifactChunk(reference.ArtifactId, 0)]);

        await store.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = "large", PipelineSucceeded = true },
            CancellationToken.None);

        await Assert.That(await connection.ExistsAsync(keys.ArtifactChunk(reference.ArtifactId, 299))).IsFalse();
    }

    // Returns its content, then fails, like a source that breaks part way through an upload.
    private sealed class FailAfterStream(byte[] content) : MemoryStream(content)
    {
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var read = await base.ReadAsync(buffer, cancellationToken);
            return read > 0 ? read : throw new IOException("Simulated read failure");
        }
    }

    [Test]
    public async Task ModuleCache_RoundTrips()
    {
        await using var connection = await ConnectAsync();
        var cache = new RedisModuleCache(
            new RedisConnectionProvider(connection),
            new RedisOptions
            {
                KeyPrefix = $"modpipe-integration-{Guid.NewGuid():N}",
                ChunkSizeBytes = 4,
                TimeToLive = TimeSpan.FromMinutes(1),
            },
            new ModuleCacheOptions());
        var data = Enumerable.Range(0, 10).Select(value => (byte) value).ToArray();

        await using (var source = new MemoryStream(data))
        {
            await cache.WriteAsync(Fingerprint, source, CancellationToken.None);
        }

        await using var restored = await cache.OpenReadAsync(Fingerprint, CancellationToken.None);
        using var copy = new MemoryStream();
        await restored!.CopyToAsync(copy);

        await Assert.That(copy.ToArray()).IsEquivalentTo(data);
    }

    private static async Task<RespireClient> ConnectAsync()
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException($"Set {ConnectionStringVariable} to run real Redis storage tests.");
        }

        return await RespireClient.ConnectAsync(connectionString);
    }
}
