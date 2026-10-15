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
    [Arguments(false)]
    [Arguments(true)]
    public async Task Lists_And_Cleans_Artifacts_Across_Scan_Pages(bool separateModules)
    {
        await using var connection = await ConnectAsync();
        var keys = new RedisKeyBuilder("modpipe-integration", Guid.NewGuid().ToString("N"));
        var store = new RedisDistributedArtifactStore(connection, keys, new RedisOptions());
        var references = new List<ArtifactReference>();
        for (var index = 0; index < 600; index++)
        {
            using var stream = new MemoryStream([1]);
            references.Add(await store.UploadAsync(
                new ArtifactDescriptor { Name = $"artifact-{index}", ModuleId = separateModules ? $"Module.{index}" : "Module.Shared" },
                stream,
                CancellationToken.None));
        }

        foreach (var group in references.GroupBy(reference => reference.ModuleId))
        {
            var listed = await store.ListArtifactsAsync(group.Key, CancellationToken.None);
            await Assert.That(listed.Select(reference => reference.ArtifactId))
                .IsEquivalentTo(group.Select(reference => reference.ArtifactId));
        }

        await store.CleanupRunAsync(new DistributedRunCleanupContext { RunId = "scan-pages", PipelineSucceeded = true }, CancellationToken.None);
        foreach (var reference in references)
        {
            await Assert.That(await connection.ExistsAsync(keys.ArtifactMeta(reference.ArtifactId))).IsFalse();
            await Assert.That(await connection.ExistsAsync(keys.ArtifactData(reference.ArtifactId))).IsFalse();
            await Assert.That(await connection.ExistsAsync(keys.ArtifactIndex(reference.ModuleId))).IsFalse();
        }

        await Assert.That(await connection.ExistsAsync(keys.ArtifactModules)).IsFalse();
    }

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
        var artifactId = await connection.Sets.ScanAsync(keys.ArtifactIndex("Integration.Partial")).SingleAsync();
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
    [Arguments(0)]
    [Arguments(7)]
    [Arguments(16)]
    [Arguments(40)]
    [Arguments(4096)]
    public async Task Master_Heartbeat_Renews_All_Artifact_Keys_In_Only_Its_Run(int length)
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions { ChunkSizeBytes = 16 };
        var keys = new RedisKeyBuilder("modpipe-integration", Guid.NewGuid().ToString("N"));
        var otherKeys = new RedisKeyBuilder("modpipe-integration", Guid.NewGuid().ToString("N"));
        var store = new RedisDistributedArtifactStore(connection, keys, options);
        var otherStore = new RedisDistributedArtifactStore(connection, otherKeys, options);
        await using var coordinator = new RedisDistributedCoordinator(connection, keys, options);
        using var source = new MemoryStream(new byte[length]);
        using var otherSource = new MemoryStream([1]);
        var reference = await store.UploadAsync(
            new ArtifactDescriptor { Name = "retained", ModuleId = "Integration.Worker" }, source, CancellationToken.None);
        var otherReference = await otherStore.UploadAsync(
            new ArtifactDescriptor { Name = "other", ModuleId = "Integration.Worker" }, otherSource, CancellationToken.None);
        string[] artifactKeys =
        [
            keys.ArtifactKeys,
            keys.ArtifactModules,
            keys.ArtifactIndex(reference.ModuleId),
            keys.ArtifactMeta(reference.ArtifactId),
            .. length < 16
                ? [keys.ArtifactData(reference.ArtifactId)]
                : Enumerable.Range(0, (length + 15) / 16).Select(index => keys.ArtifactChunk(reference.ArtifactId, index)),
        ];
        foreach (var key in artifactKeys)
        {
            await connection.ExpireAsync(key, TimeSpan.FromSeconds(5));
        }

        var otherKey = otherKeys.ArtifactData(otherReference.ArtifactId);
        await connection.ExpireAsync(otherKey, TimeSpan.FromSeconds(5));
        // The master has its own coordinator, separate from the worker's artifact store.
        await coordinator.SendMasterHeartbeatAsync(CancellationToken.None);

        foreach (var key in artifactKeys)
        {
            var expiry = await connection.Keys.ExpiryAsync(key);
            await Assert.That(expiry.TimeToLive!.Value).IsGreaterThan(TimeSpan.FromMinutes(30));
        }

        await Assert.That((await connection.Keys.ExpiryAsync(otherKey)).TimeToLive!.Value)
            .IsLessThanOrEqualTo(TimeSpan.FromSeconds(5));
        await Assert.That(await store.ListArtifactsAsync(reference.ModuleId, CancellationToken.None)).HasSingleItem();
        await using var downloaded = await store.DownloadAsync(reference, CancellationToken.None);
        await Assert.That(downloaded.Length).IsEqualTo(length);
        await store.CleanupRunAsync(new DistributedRunCleanupContext { RunId = "retained", PipelineSucceeded = true }, CancellationToken.None);
        // A later heartbeat cannot recreate deleted artifacts or indexes.
        await coordinator.SendMasterHeartbeatAsync(CancellationToken.None);
        foreach (var key in artifactKeys)
        {
            await Assert.That(await connection.ExistsAsync(key)).IsFalse();
        }
    }

    [Test]
    public async Task Artifacts_Survive_Their_Upload_Ttl_And_Expire_After_Master_Stops()
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions { ChunkSizeBytes = 16, TimeToLive = TimeSpan.FromSeconds(2) };
        var keys = new RedisKeyBuilder("modpipe-integration", Guid.NewGuid().ToString("N"));
        var store = new RedisDistributedArtifactStore(connection, keys, options);
        await using var coordinator = new RedisDistributedCoordinator(
            connection, keys, options, distributedOptions: new DistributedOptions
            {
                WorkerTimeout = TimeSpan.FromMilliseconds(100),
                MasterTimeout = TimeSpan.FromMilliseconds(200),
                ModuleResultTimeout = Timeout.InfiniteTimeSpan,
            });
        coordinator.StartMasterHeartbeat(TimeSpan.FromMilliseconds(50));
        using var source = new MemoryStream(new byte[40]);
        var reference = await store.UploadAsync(
            new ArtifactDescriptor { Name = "long-run", ModuleId = "Integration.Worker" }, source, CancellationToken.None);

        await Task.Delay(TimeSpan.FromSeconds(3));

        await Assert.That(await store.ListArtifactsAsync(reference.ModuleId, CancellationToken.None)).HasSingleItem();
        await using var downloaded = await store.DownloadAsync(reference, CancellationToken.None);
        await Assert.That(downloaded.Length).IsEqualTo(40);
        await coordinator.DisposeAsync();
        await Task.Delay(TimeSpan.FromSeconds(3));

        string[] expiredKeys =
        [
            keys.ArtifactKeys,
            keys.ArtifactModules,
            keys.ArtifactIndex(reference.ModuleId),
            keys.ArtifactMeta(reference.ArtifactId),
            keys.ArtifactChunk(reference.ArtifactId, 0),
            keys.ArtifactChunk(reference.ArtifactId, 1),
            keys.ArtifactChunk(reference.ArtifactId, 2),
        ];
        foreach (var key in expiredKeys)
        {
            await Assert.That(await connection.ExistsAsync(key)).IsFalse();
        }
    }

    [Test]
    public async Task Master_Heartbeat_Renews_Chunks_Of_Incomplete_Uploads()
    {
        await using var connection = await ConnectAsync();
        var options = new RedisOptions { ChunkSizeBytes = 16 };
        var keys = new RedisKeyBuilder("modpipe-integration", Guid.NewGuid().ToString("N"));
        var store = new RedisDistributedArtifactStore(connection, keys, options);
        await using var coordinator = new RedisDistributedCoordinator(connection, keys, options);
        await using var source = new FailAfterStream(new byte[16]);
        await Assert.That(async () => await store.UploadAsync(
                new ArtifactDescriptor { Name = "partial", ModuleId = "Integration.Partial" }, source, CancellationToken.None))
            .Throws<IOException>();
        var artifactId = await connection.Sets.ScanAsync(keys.ArtifactIndex("Integration.Partial")).SingleAsync();
        var chunkKey = keys.ArtifactChunk(artifactId, 0);
        await connection.ExpireAsync(chunkKey, TimeSpan.FromSeconds(5));

        await coordinator.SendMasterHeartbeatAsync(CancellationToken.None);

        await Assert.That((await connection.Keys.ExpiryAsync(chunkKey)).TimeToLive!.Value)
            .IsGreaterThan(TimeSpan.FromMinutes(30));
        await store.CleanupRunAsync(new DistributedRunCleanupContext { RunId = "partial", PipelineSucceeded = false }, CancellationToken.None);
        await Assert.That(await connection.ExistsAsync(chunkKey)).IsFalse();
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
