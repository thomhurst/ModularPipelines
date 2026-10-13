using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Coordination;
using Respire;
using Respire.Testing;

namespace ModularPipelines.Distributed.Redis.UnitTests.Artifacts;

public class RedisArtifactStoreTests
{
    // The fake client has no Respire retry policy, so these tests exercise the store's own retries. After a
    // dropped connection, the next command fails at once while the connection is replaced; back off long
    // enough for the replacement to finish, as production does.
    private static readonly TimeSpan RetryBaseDelay = TimeSpan.FromMilliseconds(200);

    private FakeRedis _redis = null!;
    private RedisKeyBuilder _keys = null!;
    private RedisDistributedArtifactStore _store = null!;

    [Before(Test)]
    public async Task Setup()
    {
        _redis = await FakeRedis.StartAsync();
        _keys = new RedisKeyBuilder("modpipe", "run123");
        _store = CreateStore(RetryBaseDelay);
    }

    [After(Test)]
    public async Task Cleanup() => await _redis.DisposeAsync();

    [Test]
    public async Task Upload_SmallArtifact_ReturnsCorrectReference()
    {
        var descriptor = new ArtifactDescriptor
        {
            Name = "test-art",
            ModuleId = "Test.Module",
            ContentType = "application/octet-stream",
        };
        var data = new byte[] { 1, 2, 3, 4, 5 };

        using var stream = new MemoryStream(data);
        var reference = await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(reference.Name).IsEqualTo("test-art");
        await Assert.That(reference.ModuleId.Value).IsEqualTo("Test.Module");
        await Assert.That(reference.SizeBytes).IsEqualTo(5);
        await Assert.That(reference.ContentType).IsEqualTo("application/octet-stream");
        await Assert.That(reference.ArtifactId).IsNotNull();
    }

    [Test]
    public async Task Upload_Indexes_The_Artifact_With_The_Time_To_Live()
    {
        using var stream = new MemoryStream([1]);
        var reference = await _store.UploadAsync(
            new ArtifactDescriptor { Name = "test-art", ModuleId = "Test.Module" },
            stream,
            CancellationToken.None);

        var indexExpiry = await _redis.Client.Keys.ExpiryAsync(_keys.ArtifactIndex("Test.Module"));
        var modulesExpiry = await _redis.Client.Keys.ExpiryAsync(_keys.ArtifactModules);
        using (Assert.Multiple())
        {
            await Assert.That(await _redis.Client.Sets.MembersAsync(_keys.ArtifactIndex("Test.Module")))
                .IsEquivalentTo([reference.ArtifactId]);
            await Assert.That(await _redis.Client.Sets.MembersAsync(_keys.ArtifactModules))
                .IsEquivalentTo(["Test.Module"]);
            await Assert.That(indexExpiry.TimeToLive).IsNotNull().And.IsLessThanOrEqualTo(TimeSpan.FromHours(1));
            await Assert.That(modulesExpiry.TimeToLive).IsNotNull().And.IsLessThanOrEqualTo(TimeSpan.FromHours(1));
        }
    }

    [Test]
    public async Task Upload_Indexes_The_Artifact_Before_A_Failed_Content_Read()
    {
        await using var stream = new FailingStream();

        await Assert.That(async () => await _store.UploadAsync(
                new ArtifactDescriptor { Name = "test-art", ModuleId = "Test.Module" },
                stream,
                CancellationToken.None))
            .Throws<IOException>();

        // Run cleanup finds the partial upload through the index.
        await Assert.That(await _redis.Client.Sets.MembersAsync(_keys.ArtifactIndex("Test.Module"))).HasSingleItem();
        await Assert.That(await _redis.Client.Sets.MembersAsync(_keys.ArtifactModules)).IsEquivalentTo(["Test.Module"]);
    }

    [Test]
    public async Task Upload_Surfaces_A_Failed_Index_Command()
    {
        // EXEC succeeds while SADD fails on the wrong key type, so the store must read each queued result.
        await _redis.Client.SetAsync(_keys.ArtifactModules, (RespireValue) "not-a-set");
        using var stream = new MemoryStream([1]);

        await Assert.That(async () => await _store.UploadAsync(
                new ArtifactDescriptor { Name = "test-art", ModuleId = "Test.Module" },
                stream,
                CancellationToken.None))
            .Throws<RespireServerException>()
            .WithMessageContaining("WRONGTYPE");
    }

    private sealed class FailingStream : MemoryStream
    {
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
            ValueTask.FromException<int>(new IOException("Simulated read failure"));
    }

    [Test]
    public async Task Upload_LargeArtifact_Is_Chunked()
    {
        var descriptor = new ArtifactDescriptor
        {
            Name = "big-art",
            ModuleId = "Test.Module",
        };
        var data = new byte[150]; // Larger than ChunkSizeBytes (50)

        using var stream = new MemoryStream(data);
        var reference = await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(reference.SizeBytes).IsEqualTo(150);
            await Assert.That(reference.Name).IsEqualTo("big-art");
            await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactData(reference.ArtifactId))).IsFalse();
            await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactChunk(reference.ArtifactId, 2))).IsTrue();
        }
    }

    [Test]
    public async Task Download_SmallArtifact_RetrievesData()
    {
        var data = new byte[] { 10, 20, 30 };
        await _redis.Client.SetAsync(_keys.ArtifactData("art1"), (RespireValue) data);

        await using var result = await _store.DownloadAsync(CreateReference("art1", data.Length), CancellationToken.None);
        using var ms = new MemoryStream();
        await result.CopyToAsync(ms);

        await Assert.That(ms.ToArray()).IsEquivalentTo(data);
    }

    [Test]
    public async Task Download_ChunkedArtifact_UsesSeekableTemporaryStorage()
    {
        var data = Enumerable.Range(0, 150).Select(value => (byte) value).ToArray();
        await StoreChunksAsync("chunked", data);

        string temporaryPath;
        await using (var result = await _store.DownloadAsync(CreateReference("chunked", data.Length), CancellationToken.None))
        {
            // ZIP readers require seeking; disk backing avoids MemoryStream's 2 GB ceiling.
            await Assert.That(result).IsTypeOf<FileStream>();
            temporaryPath = ((FileStream) result).Name;
            await Assert.That(result.Position).IsEqualTo(0);
            result.Seek(75, SeekOrigin.Begin);
            await Assert.That(result.ReadByte()).IsEqualTo(75);
            result.Position = 0;
            using var copy = new MemoryStream();
            await result.CopyToAsync(copy);
            await Assert.That(copy.ToArray()).IsEquivalentTo(data);
        }

        await Assert.That(File.Exists(temporaryPath)).IsFalse();
    }

    [Test]
    public async Task Download_ChunkedArtifact_Retries_Transient_Failures()
    {
        var data = Enumerable.Range(0, 100).Select(value => (byte) value).ToArray();
        await StoreChunksAsync("flaky", data);
        var chunkKey = _keys.ArtifactChunk("flaky", 1);
        using var loading = _redis.InjectFault("GET", chunkKey, RespireFakeFault.Loading());
        using var dropped = _redis.InjectFault("GET", chunkKey, RespireFakeFault.Disconnect());

        await using var result = await _store.DownloadAsync(CreateReference("flaky", data.Length), CancellationToken.None);
        using var copy = new MemoryStream();
        await result.CopyToAsync(copy);

        await Assert.That(copy.ToArray()).IsEquivalentTo(data);
        await Assert.That(loading.MatchedCount).IsEqualTo(1);
        await Assert.That(dropped.MatchedCount).IsEqualTo(1);
    }

    [Test]
    public async Task Download_Surfaces_The_Failure_After_Every_Attempt_Fails()
    {
        using var fault = _redis.InjectFault("GET", _keys.ArtifactData("down"), RespireFakeFault.Loading(), occurrences: null);

        await Assert.That(async () => await _store.DownloadAsync(CreateReference("down", 3), CancellationToken.None))
            .Throws<RespireServerException>()
            .WithMessageContaining("LOADING");
        await Assert.That(fault.MatchedCount).IsEqualTo(RedisTransientRetry.MaxAttempts);
    }

    [Test]
    public async Task Download_Does_Not_Retry_Server_Errors()
    {
        using var fault = _redis.InjectFault("GET", _keys.ArtifactData("denied"), RespireFakeFault.ReadOnly(), occurrences: null);

        await Assert.That(async () => await _store.DownloadAsync(CreateReference("denied", 3), CancellationToken.None))
            .Throws<RespireServerException>();
        await Assert.That(fault.MatchedCount).IsEqualTo(1);
    }

    [Test]
    public async Task Download_Cancellation_Stops_Pending_Retry_Backoff()
    {
        using var cancellation = new CancellationTokenSource();
        var store = CreateStore(retryBaseDelay: TimeSpan.FromMinutes(5));
        using var fault = _redis.InjectFault("GET", _keys.ArtifactData("slow"), RespireFakeFault.Disconnect(), occurrences: null);
        _ = fault.Matched.ContinueWith(
            _ => cancellation.CancelAfter(TimeSpan.FromSeconds(2)),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

        // The first retry would wait minutes; cancellation must end that wait instead. Cancelling
        // before the retry decision surfaces the failure itself, so the delay leaves a cold, loaded
        // runner time to enter the backoff first.
        await Assert.That(async () => await store.DownloadAsync(CreateReference("slow", 3), cancellation.Token)
                .WaitAsync(TimeSpan.FromSeconds(30)))
            .Throws<OperationCanceledException>();
        await Assert.That(fault.MatchedCount).IsEqualTo(1);
    }

    [Test]
    public async Task Upload_Chunk_Retries_Dropped_Connection()
    {
        // The artifact identifier is random, so match the first chunk write by command alone: the
        // index and metadata are written with other commands.
        using var fault = _redis.Server.InjectFault("SET", RespireFakeFault.Disconnect());
        var data = Enumerable.Range(0, 120).Select(value => (byte) value).ToArray();
        using var stream = new MemoryStream(data);

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "chunked", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        await Assert.That(fault.MatchedCount).IsEqualTo(1);
        await Assert.That(await DownloadAsync(reference)).IsEquivalentTo(data);
    }

    [Test]
    public async Task Upload_Retry_After_An_Ambiguous_Write_Stores_The_Same_Bytes()
    {
        // The first attempt is applied but its reply is lost, so the retry rewrites the chunk. It must
        // write the same bytes even though the upload has refilled its buffer with the next chunk.
        using var fault = _redis.Server.InjectFault("SET", RespireFakeFault.Disconnect(afterExecution: true));
        var data = Enumerable.Range(0, 100).Select(value => (byte) value).ToArray();
        using var stream = new MemoryStream(data);

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "stable", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        await Assert.That(fault.ExecutionCount).IsEqualTo(1);
        await Assert.That(await _redis.Client.GetBytesAsync(_keys.ArtifactChunk(reference.ArtifactId, 0)))
            .IsEquivalentTo(data.Take(50).ToArray());
    }

    [Test]
    public async Task Download_ObservesCancellationWhileRedisReadIsPending()
    {
        var gate = new RespireFakeGate();
        using var fault = _redis.InjectFault("GET", _keys.ArtifactChunk("pending", 0), RespireFakeFault.Pause(gate));
        using var cancellation = new CancellationTokenSource();
        _ = fault.Matched.ContinueWith(
            _ => cancellation.Cancel(),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

        var download = _store.DownloadAsync(CreateReference("pending", 50), cancellation.Token);
        try
        {
            await Assert.That(async () => await download.WaitAsync(TimeSpan.FromSeconds(5)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            gate.Release();
            try
            {
                await download;
            }
            catch (Exception)
            {
                // Observe the failed download even when the regression assertion fails.
            }
        }
    }

    [Test]
    public async Task ListArtifacts_ReturnsStoredReferences()
    {
        using var stream = new MemoryStream([1, 2, 3]);
        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "art1", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        var results = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        await Assert.That(results.Count).IsEqualTo(1);
        await Assert.That(results[0].Name).IsEqualTo("art1");
        await Assert.That(results[0].ArtifactId).IsEqualTo(reference.ArtifactId);
    }

    [Test]
    [Arguments(3)]
    [Arguments(120)]
    public async Task Delete_Removes_Content_Metadata_And_Index_Entry(int length)
    {
        using var stream = new MemoryStream(new byte[length]);
        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "test", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        await _store.DeleteAsync(reference, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactMeta(reference.ArtifactId))).IsFalse();
            await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactData(reference.ArtifactId))).IsFalse();
            await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactChunk(reference.ArtifactId, 0))).IsFalse();
            await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactChunk(reference.ArtifactId, 2))).IsFalse();
            await Assert.That(await _redis.Client.Sets.MembersAsync(_keys.ArtifactIndex("Test.Module"))).IsEmpty();
        }
    }

    [Test]
    public async Task Run_Cleanup_Deletes_Every_Artifact_And_Index()
    {
        using var small = new MemoryStream([1, 2, 3]);
        using var chunked = new MemoryStream(new byte[120]);
        var smallReference = await _store.UploadAsync(new ArtifactDescriptor { Name = "small", ModuleId = "Test.One" }, small, CancellationToken.None);
        var chunkedReference = await _store.UploadAsync(new ArtifactDescriptor { Name = "chunked", ModuleId = "Test.Two" }, chunked, CancellationToken.None);

        await _store.CleanupRunAsync(new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true }, CancellationToken.None);

        string[] deletedKeys =
        [
            _keys.ArtifactMeta(smallReference.ArtifactId),
            _keys.ArtifactData(smallReference.ArtifactId),
            _keys.ArtifactMeta(chunkedReference.ArtifactId),
            _keys.ArtifactChunk(chunkedReference.ArtifactId, 0),
            _keys.ArtifactChunk(chunkedReference.ArtifactId, 2),
            _keys.ArtifactIndex("Test.One"),
            _keys.ArtifactIndex("Test.Two"),
            _keys.ArtifactModules,
        ];
        foreach (var key in deletedKeys)
        {
            await Assert.That(await _redis.Client.ExistsAsync(key)).IsFalse();
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(50)]
    [Arguments(120)]
    [Arguments(1001)]
    public async Task Upload_Then_Download_RoundTrips(int length)
    {
        var data = Enumerable.Range(0, length).Select(value => (byte) value).ToArray();
        using var stream = new MemoryStream(data);

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "round-trip", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(reference.SizeBytes).IsEqualTo(length);
            await Assert.That(await DownloadAsync(reference)).IsEquivalentTo(data);
        }
    }

    [Test]
    public async Task Upload_EmptyArtifact_WritesSingleKey()
    {
        using var stream = new MemoryStream();

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "empty", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        await Assert.That(await _redis.Client.GetBytesAsync(_keys.ArtifactData(reference.ArtifactId))).IsEmpty();
        await Assert.That(await _redis.Client.ExistsAsync(_keys.ArtifactChunk(reference.ArtifactId, 0))).IsFalse();
    }

    [Test]
    public async Task Download_SingleKey_Rejects_Size_Mismatch()
    {
        await _redis.Client.SetAsync(_keys.ArtifactData("short"), (RespireValue) new byte[] { 1, 2, 3 });

        await Assert.That(async () => await _store.DownloadAsync(CreateReference("short", 10), CancellationToken.None))
            .Throws<InvalidOperationException>()
            .WithMessageContaining("size mismatch");
    }

    [Test]
    public async Task Upload_ObservesCancellationWhileRedisWriteIsPending()
    {
        var gate = new RespireFakeGate();
        using var fault = _redis.Server.InjectFault("SET", RespireFakeFault.Pause(gate));
        using var cancellation = new CancellationTokenSource();
        _ = fault.Matched.ContinueWith(
            _ => cancellation.Cancel(),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
        using var stream = new MemoryStream([1, 2, 3]);

        try
        {
            await Assert.That(async () => await _store
                    .UploadAsync(new ArtifactDescriptor { Name = "pending", ModuleId = "Test.Module" }, stream, cancellation.Token)
                    .WaitAsync(TimeSpan.FromSeconds(5)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            gate.Release();
        }
    }

    [Test]
    public async Task ListArtifacts_ObservesCancellationWhileRedisReadIsPending()
    {
        var gate = new RespireFakeGate();
        using var fault = _redis.Server.InjectFault("SMEMBERS", RespireFakeFault.Pause(gate));
        using var cancellation = new CancellationTokenSource();
        _ = fault.Matched.ContinueWith(
            _ => cancellation.Cancel(),
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);

        try
        {
            await Assert.That(async () => await _store
                    .ListArtifactsAsync("Test.Module", cancellation.Token)
                    .WaitAsync(TimeSpan.FromSeconds(5)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            gate.Release();
        }
    }

    [Test]
    public async Task ArtifactKeyBuilder_GeneratesCorrectPatterns()
    {
        var keys = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(keys.ArtifactMeta("art1")).IsEqualTo("modpipe:{abc123}:artifacts:meta:art1");
        await Assert.That(keys.ArtifactData("art1")).IsEqualTo("modpipe:{abc123}:artifacts:data:art1");
        await Assert.That(keys.ArtifactChunk("art1", 0)).IsEqualTo("modpipe:{abc123}:artifacts:data:art1:chunk:0");
        await Assert.That(keys.ArtifactChunk("art1", 3)).IsEqualTo("modpipe:{abc123}:artifacts:data:art1:chunk:3");
        await Assert.That(keys.ArtifactIndex("My.Module")).IsEqualTo("modpipe:{abc123}:artifacts:index:My.Module");
    }

    private RedisDistributedArtifactStore CreateStore(TimeSpan retryBaseDelay) =>
        new(
            _redis.Client,
            _keys,
            new RedisOptions
            {
                ChunkSizeBytes = 50,
                TimeToLive = TimeSpan.FromHours(1),
            },
            retryBaseDelay);

    private static ArtifactReference CreateReference(string artifactId, long sizeBytes) => new()
    {
        ArtifactId = artifactId,
        Name = "test",
        ModuleId = "Test.Module",
        SizeBytes = sizeBytes,
        UploadedAt = DateTimeOffset.UtcNow,
    };

    private async Task StoreChunksAsync(string artifactId, byte[] data)
    {
        for (var index = 0; index * 50 < data.Length; index++)
        {
            await _redis.Client.SetAsync(
                _keys.ArtifactChunk(artifactId, index),
                (RespireValue) data.Skip(index * 50).Take(50).ToArray());
        }
    }

    private async Task<byte[]> DownloadAsync(ArtifactReference reference)
    {
        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var copy = new MemoryStream();
        await result.CopyToAsync(copy);
        return copy.ToArray();
    }
}
