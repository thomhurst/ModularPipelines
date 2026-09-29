using Moq;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Coordination;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Artifacts;

public class RedisArtifactStoreTests
{
    private Mock<IDatabase> _mockDb = null!;
    private RedisKeyBuilder _keys = null!;
    private RedisDistributedArtifactStore _store = null!;

    [Before(Test)]
    public void Setup()
    {
        _mockDb = new Mock<IDatabase>(MockBehavior.Loose);
        _keys = new RedisKeyBuilder("modpipe", "run123");
        var options = new RedisOptions
        {
            ChunkSizeBytes = 50,
            TimeToLive = TimeSpan.FromHours(1),
        };
        _store = new RedisDistributedArtifactStore(_mockDb.Object, _keys, options, retryBaseDelay: TimeSpan.Zero);
    }

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
    public async Task Upload_LargeArtifact_ReturnsCorrectSize()
    {
        var descriptor = new ArtifactDescriptor
        {
            Name = "big-art",
            ModuleId = "Test.Module",
        };
        var data = new byte[150]; // Larger than ChunkSizeBytes (50)

        using var stream = new MemoryStream(data);
        var reference = await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(reference.SizeBytes).IsEqualTo(150);
        await Assert.That(reference.Name).IsEqualTo("big-art");
    }

    [Test]
    public async Task Download_SmallArtifact_RetrievesData()
    {
        var data = new byte[] { 10, 20, 30 };
        var reference = new ArtifactReference
        {
            ArtifactId = "art1",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = 3,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };

        _mockDb.Setup(db => db.StringGetAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:data:art1") && !k.ToString().Contains("chunk")),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) data);

        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var ms = new MemoryStream();
        await result.CopyToAsync(ms);

        await Assert.That(ms.ToArray()).IsEquivalentTo(data);
    }

    [Test]
    public async Task Download_ChunkedArtifact_UsesSeekableTemporaryStorage()
    {
        var data = Enumerable.Range(0, 150).Select(value => (byte) value).ToArray();
        var reference = new ArtifactReference
        {
            ArtifactId = "chunked",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = data.Length,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };
        for (var index = 0; index < 3; index++)
        {
            var chunkKey = _keys.ArtifactChunk("chunked", index);
            _mockDb.Setup(db => db.StringGetAsync(chunkKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue) data.Skip(index * 50).Take(50).ToArray());
        }

        string temporaryPath;
        await using (var result = await _store.DownloadAsync(reference, CancellationToken.None))
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
    public async Task Download_ChunkedArtifact_Retries_Transient_Timeout()
    {
        var data = Enumerable.Range(0, 100).Select(value => (byte) value).ToArray();
        var reference = new ArtifactReference
        {
            ArtifactId = "flaky",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = data.Length,
            UploadedAt = DateTimeOffset.UtcNow,
        };
        _mockDb.Setup(db => db.StringGetAsync(_keys.ArtifactChunk("flaky", 0), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) data.Take(50).ToArray());
        _mockDb.SetupSequence(db => db.StringGetAsync(_keys.ArtifactChunk("flaky", 1), It.IsAny<CommandFlags>()))
            .ThrowsAsync(new RedisTimeoutException("Timeout awaiting response", CommandStatus.Sent))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.SocketFailure, "socket closed"))
            .ReturnsAsync((RedisValue) data.Skip(50).ToArray());

        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var copy = new MemoryStream();
        await result.CopyToAsync(copy);

        await Assert.That(copy.ToArray()).IsEquivalentTo(data);
        _mockDb.Verify(db => db.StringGetAsync(_keys.ArtifactChunk("flaky", 1), It.IsAny<CommandFlags>()), Times.Exactly(3));
    }

    [Test]
    public async Task Download_Surfaces_Timeout_After_Every_Attempt_Fails()
    {
        var reference = new ArtifactReference { ArtifactId = "down", Name = "test", ModuleId = "Test.Module", SizeBytes = 3, UploadedAt = DateTimeOffset.UtcNow };
        _mockDb.Setup(db => db.StringGetAsync(_keys.ArtifactData("down"), It.IsAny<CommandFlags>()))
            .ThrowsAsync(new RedisTimeoutException("Timeout awaiting response", CommandStatus.Sent));

        await Assert.That(async () => await _store.DownloadAsync(reference, CancellationToken.None))
            .Throws<RedisTimeoutException>();
        _mockDb.Verify(
            db => db.StringGetAsync(_keys.ArtifactData("down"), It.IsAny<CommandFlags>()),
            Times.Exactly(RedisTransientRetry.MaxAttempts));
    }

    [Test]
    public async Task Download_Does_Not_Retry_Server_Errors()
    {
        var reference = new ArtifactReference { ArtifactId = "denied", Name = "test", ModuleId = "Test.Module", SizeBytes = 3, UploadedAt = DateTimeOffset.UtcNow };
        _mockDb.Setup(db => db.StringGetAsync(_keys.ArtifactData("denied"), It.IsAny<CommandFlags>()))
            .ThrowsAsync(new RedisServerException("NOAUTH Authentication required."));

        await Assert.That(async () => await _store.DownloadAsync(reference, CancellationToken.None))
            .Throws<RedisServerException>();
        _mockDb.Verify(
            db => db.StringGetAsync(_keys.ArtifactData("denied"), It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Test]
    public async Task Upload_Chunk_Retries_Transient_Timeout()
    {
        var writes = 0;
        _mockDb.Setup(db => db.StringSetAsync(
                It.Is<RedisKey>(key => key.ToString().Contains(":chunk:1")),
                It.IsAny<RedisValue>(),
                It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()))
            .Returns(() => Interlocked.Increment(ref writes) == 1
                ? Task.FromException<bool>(new RedisTimeoutException("Timeout performing SET", CommandStatus.Sent))
                : Task.FromResult(true));
        using var stream = new MemoryStream(new byte[120]);

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "chunked", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        await Assert.That(reference.SizeBytes).IsEqualTo(120);
        await Assert.That(writes).IsEqualTo(2);
    }

    [Test]
    public async Task Upload_Retry_Keeps_Timed_Out_Attempt_Bytes_Stable()
    {
        // A timed-out SET can still reach Redis later, so the bytes it references must not change
        // when the upload refills its buffer with the next chunk.
        var firstChunkAttempts = new List<RedisValue>();
        _mockDb.Setup(db => db.StringSetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":chunk:0", StringComparison.Ordinal)),
                It.IsAny<RedisValue>(),
                It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()))
            .Returns((RedisKey _, RedisValue value, Expiration _, ValueCondition _, CommandFlags _) =>
            {
                firstChunkAttempts.Add(value);
                return firstChunkAttempts.Count == 1
                    ? Task.FromException<bool>(new RedisTimeoutException("Timeout performing SET", CommandStatus.WaitingToBeSent))
                    : Task.FromResult(true);
            });
        var data = Enumerable.Range(0, 100).Select(value => (byte) value).ToArray();
        using var stream = new MemoryStream(data);

        await _store.UploadAsync(new ArtifactDescriptor { Name = "stable", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        await Assert.That(firstChunkAttempts).Count().IsEqualTo(2);
        foreach (var attempt in firstChunkAttempts)
        {
            await Assert.That(((byte[]) attempt!).ToArray()).IsEquivalentTo(data.Take(50).ToArray());
        }
    }

    [Test]
    public async Task Download_ObservesCancellationWhileRedisReadIsPending()
    {
        var pendingRead = new TaskCompletionSource<RedisValue>();
        var reference = new ArtifactReference
        {
            ArtifactId = "pending",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = 50,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };
        using var cancellation = new CancellationTokenSource();
        _mockDb.Setup(db => db.StringGetAsync(_keys.ArtifactChunk("pending", 0), It.IsAny<CommandFlags>()))
            .Callback(() => cancellation.Cancel())
            .Returns(pendingRead.Task);

        var download = _store.DownloadAsync(reference, cancellation.Token);
        try
        {
            await Assert.That(async () => await download.WaitAsync(TimeSpan.FromSeconds(2)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            pendingRead.TrySetResult(RedisValue.Null);
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
        var ref1 = new ArtifactReference
        {
            ArtifactId = "id1",
            Name = "art1",
            ModuleId = "Test.Module",
            SizeBytes = 100,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };
        var ref1Json = System.Text.Json.JsonSerializer.Serialize(ref1);

        _mockDb.Setup(db => db.SetMembersAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:index:Test.Module")),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync([new RedisValue("id1")]);

        _mockDb.Setup(db => db.StringGetAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:meta:id1")),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(ref1Json));

        var results = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        await Assert.That(results.Count).IsEqualTo(1);
        await Assert.That(results[0].Name).IsEqualTo("art1");
    }

    [Test]
    public async Task Delete_CallsKeyDeleteAndSetRemove()
    {
        var reference = new ArtifactReference
        {
            ArtifactId = "art1",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = 3,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };

        await _store.DeleteAsync(reference, CancellationToken.None);

        // Verify meta key deleted
        _mockDb.Verify(db => db.KeyDeleteAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:meta:art1")),
            It.IsAny<CommandFlags>()), Times.Once);

        // Verify data key deleted
        _mockDb.Verify(db => db.KeyDeleteAsync(
            It.Is<RedisKey>(k => k.ToString() == _keys.ArtifactData("art1")),
            It.IsAny<CommandFlags>()), Times.Once);

        // Verify index updated
        _mockDb.Verify(db => db.SetRemoveAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:index:Test.Module")),
            It.Is<RedisValue>(v => v.ToString() == "art1"),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    [Arguments(0)]
    [Arguments(5)]
    [Arguments(50)]
    [Arguments(120)]
    public async Task Upload_Then_Download_RoundTrips(int length)
    {
        var data = Enumerable.Range(0, length).Select(value => (byte) value).ToArray();
        using var stream = new MemoryStream(data);
        ServeWrittenValues();

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "round-trip", ModuleId = "Test.Module" }, stream, CancellationToken.None);
        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var copy = new MemoryStream();
        await result.CopyToAsync(copy);

        using (Assert.Multiple())
        {
            await Assert.That(reference.SizeBytes).IsEqualTo(length);
            await Assert.That(copy.ToArray()).IsEquivalentTo(data);
        }
    }

    [Test]
    public async Task Upload_EmptyArtifact_WritesSingleKey()
    {
        using var stream = new MemoryStream();

        var reference = await _store.UploadAsync(new ArtifactDescriptor { Name = "empty", ModuleId = "Test.Module" }, stream, CancellationToken.None);

        _mockDb.Verify(db => db.StringSetAsync(
            It.Is<RedisKey>(key => key.ToString() == _keys.ArtifactData(reference.ArtifactId)),
            It.IsAny<RedisValue>(),
            It.IsAny<Expiration>(),
            It.IsAny<ValueCondition>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task Download_SingleKey_Rejects_Size_Mismatch()
    {
        var reference = new ArtifactReference { ArtifactId = "short", Name = "test", ModuleId = "Test.Module", SizeBytes = 10, UploadedAt = DateTimeOffset.UtcNow };
        _mockDb.Setup(db => db.StringGetAsync(_keys.ArtifactData("short"), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) new byte[] { 1, 2, 3 });

        await Assert.That(async () => await _store.DownloadAsync(reference, CancellationToken.None))
            .Throws<InvalidOperationException>()
            .WithMessageContaining("size mismatch");
    }

    [Test]
    public async Task Upload_ObservesCancellationWhileRedisWriteIsPending()
    {
        var pendingWrite = new TaskCompletionSource<bool>();
        using var cancellation = new CancellationTokenSource();
        _mockDb.Setup(db => db.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()))
            .Callback(() => cancellation.Cancel())
            .Returns(pendingWrite.Task);
        using var stream = new MemoryStream([1, 2, 3]);

        try
        {
            await Assert.That(async () => await _store
                    .UploadAsync(new ArtifactDescriptor { Name = "pending", ModuleId = "Test.Module" }, stream, cancellation.Token)
                    .WaitAsync(TimeSpan.FromSeconds(2)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            pendingWrite.TrySetResult(true);
        }
    }

    [Test]
    public async Task ListArtifacts_ObservesCancellationWhileRedisReadIsPending()
    {
        var pendingRead = new TaskCompletionSource<RedisValue[]>();
        using var cancellation = new CancellationTokenSource();
        _mockDb.Setup(db => db.SetMembersAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Callback(() => cancellation.Cancel())
            .Returns(pendingRead.Task);

        try
        {
            await Assert.That(async () => await _store
                    .ListArtifactsAsync("Test.Module", cancellation.Token)
                    .WaitAsync(TimeSpan.FromSeconds(2)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            pendingRead.TrySetResult([]);
        }
    }

    private void ServeWrittenValues()
    {
        // The store reuses its chunk buffer, so copy each value when it is written, as Redis does.
        var values = new Dictionary<string, byte[]>();
        _mockDb.Setup(db => db.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()))
            .Callback((RedisKey key, RedisValue value, Expiration _, ValueCondition _, CommandFlags _) =>
                values[key.ToString()] = [.. (byte[]?) value ?? []])
            .ReturnsAsync(true);
        _mockDb.Setup(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisKey key, CommandFlags _) =>
                values.TryGetValue(key.ToString(), out var value) ? (RedisValue) value : RedisValue.Null);
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
}
