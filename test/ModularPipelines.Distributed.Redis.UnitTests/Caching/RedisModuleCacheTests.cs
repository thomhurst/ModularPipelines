using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Caching;
using Respire;
using Respire.Testing;

namespace ModularPipelines.Distributed.Redis.UnitTests.Caching;

public class RedisModuleCacheTests
{
    private const string Fingerprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    private static readonly string Generation = new('b', 32);
    private static readonly string FingerprintPrefix = $"custom-prefix:module-cache:v2:{{{Fingerprint.ToLowerInvariant()}}}";
    private static readonly string MetadataKey = $"{FingerprintPrefix}:metadata";
    private FakeRedis _redis = null!;
    private RedisModuleCache _cache = null!;

    [Before(Test)]
    public async Task Setup()
    {
        _redis = await FakeRedis.StartAsync();
        _cache = CreateCache(maximumCacheEntryBytes: new ModuleCacheOptions().MaxCacheEntryBytes);
    }

    [After(Test)]
    public async Task Cleanup() => await _redis.DisposeAsync();

    [Test]
    public async Task WriteUsesChunkedStableCrossRunKeys()
    {
        await using var content = new MemoryStream([1, 2, 3, 4, 5]);

        await _cache.WriteAsync(Fingerprint, content, CancellationToken.None);

        // The fingerprint is a hash tag, so all of an entry's keys map to one Redis Cluster slot.
        var metadata = await _redis.Client.GetStringAsync(MetadataKey);
        await Assert.That(metadata).IsNotNull();
        var generation = metadata![..metadata.IndexOf(':', StringComparison.Ordinal)];
        var firstChunk = $"{FingerprintPrefix}:entry:{generation}:chunk:0";
        var secondChunk = $"{FingerprintPrefix}:entry:{generation}:chunk:1";
        using (Assert.Multiple())
        {
            await Assert.That(metadata).IsEqualTo($"{generation}:2:5");
            await Assert.That(await _redis.Client.GetBytesAsync(firstChunk)).IsEquivalentTo(new byte[] { 1, 2, 3 });
            await Assert.That(await _redis.Client.GetBytesAsync(secondChunk)).IsEquivalentTo(new byte[] { 4, 5 });
            await Assert.That(await _redis.Client.ExistsAsync($"{FingerprintPrefix}:entry:{generation}:chunk:2")).IsFalse();

            // Publishing replaces the one-hour provisional chunk expiry with the configured time to live.
            foreach (var key in new[] { MetadataKey, firstChunk, secondChunk })
            {
                var expiry = await _redis.Client.Keys.ExpiryAsync(key);
                await Assert.That(expiry.TimeToLive).IsNotNull().And.IsLessThanOrEqualTo(TimeSpan.FromSeconds(60));
            }
        }
    }

    [Test]
    public async Task OpenReadReassemblesChunks()
    {
        await StoreEntryAsync($"{Generation}:2:5", [1, 2, 3], [4, 5]);

        await using var result = await _cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        using var destination = new MemoryStream();
        await result!.CopyToAsync(destination);
        await Assert.That(destination.ToArray()).IsEquivalentTo(new byte[] { 1, 2, 3, 4, 5 });
    }

    [Test]
    public async Task WriteThenOpenReadRoundTrips()
    {
        var data = Enumerable.Range(0, 10).Select(value => (byte) value).ToArray();
        await using (var content = new MemoryStream(data))
        {
            await _cache.WriteAsync(Fingerprint, content, CancellationToken.None);
        }

        await using var result = await _cache.OpenReadAsync(Fingerprint, CancellationToken.None);
        using var destination = new MemoryStream();
        await result!.CopyToAsync(destination);

        await Assert.That(destination.ToArray()).IsEquivalentTo(data);
    }

    [Test]
    public async Task FailedWriteDeletesItsChunksAndPublishesNothing()
    {
        await using var content = new FailAfterStream([1, 2, 3, 4]);
        using var deletes = _redis.Server.InjectFault("DEL", RespireFakeFault.Delay(TimeSpan.Zero), occurrences: null);

        await Assert.That(async () => await _cache.WriteAsync(Fingerprint, content, CancellationToken.None))
            .Throws<IOException>();

        using (Assert.Multiple())
        {
            // Both written chunks are deleted together.
            await Assert.That(deletes.ExecutionCount).IsEqualTo(1);
            await Assert.That(await _redis.Client.ExistsAsync(MetadataKey)).IsFalse();
            await Assert.That(await _cache.ExistsAsync(Fingerprint, CancellationToken.None)).IsFalse();
        }
    }

    [Test]
    public async Task DeleteRemovesMetadataThenCurrentGenerationChunks()
    {
        await StoreEntryAsync($"{Generation}:2:5", [1, 2, 3], [4, 5]);
        var gate = new RespireFakeGate();
        using var chunkDelete = _redis.InjectFault("DEL", ChunkKey(0), RespireFakeFault.Pause(gate));

        var delete = _cache.DeleteAsync(Fingerprint, CancellationToken.None);
        bool metadataDeletedFirst;
        bool chunkPresentBeforeDelete;
        try
        {
            await chunkDelete.Matched.WaitAsync(TimeSpan.FromSeconds(5));

            // The paused connection holds later replies, so observe through a second connection.
            await using var observer = await RespireClient.ConnectAsync(_redis.Server.CreateOptions());
            metadataDeletedFirst = !await observer.ExistsAsync(MetadataKey);
            chunkPresentBeforeDelete = await observer.ExistsAsync(ChunkKey(0));
        }
        finally
        {
            gate.Release();
        }

        await delete;

        using (Assert.Multiple())
        {
            // Readers never observe an entry with missing chunks.
            await Assert.That(metadataDeletedFirst).IsTrue();
            await Assert.That(chunkPresentBeforeDelete).IsTrue();
            await Assert.That(await _redis.Client.ExistsAsync(ChunkKey(0))).IsFalse();
            await Assert.That(await _redis.Client.ExistsAsync(ChunkKey(1))).IsFalse();
        }
    }

    [Test]
    public async Task DeleteIgnoresMissingEntryAndExistsChecksMetadata()
    {
        using var deletes = _redis.Server.InjectFault("DEL", RespireFakeFault.Delay(TimeSpan.Zero), occurrences: null);

        await _cache.DeleteAsync(Fingerprint, CancellationToken.None);
        await Assert.That(deletes.MatchedCount).IsEqualTo(0);
        await Assert.That(await _cache.ExistsAsync(Fingerprint, CancellationToken.None)).IsFalse();

        await _redis.Client.SetAsync(MetadataKey, (RespireValue) $"{Generation}:0:0");
        await Assert.That(await _cache.ExistsAsync(Fingerprint, CancellationToken.None)).IsTrue();
    }

    [Test]
    public async Task OpenReadReturnsNullWhenMetadataIsMissing()
    {
        var result = await _cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task OpenReadRejectsMetadataLengthAboveConfiguredLimit()
    {
        await StoreEntryAsync($"{Generation}:1:4", [1, 2, 3, 4]);
        using var chunkReads = _redis.InjectFault("GET", ChunkKey(0), RespireFakeFault.Delay(TimeSpan.Zero), occurrences: null);
        var cache = CreateCache(maximumCacheEntryBytes: 3);

        await Assert.That(async () =>
                await cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>();

        await Assert.That(chunkReads.MatchedCount).IsEqualTo(0);
    }

    [Test]
    public async Task OpenReadRejectsChunksAboveConfiguredLimit()
    {
        await StoreEntryAsync($"{Generation}:2:3", [1, 2], [1, 2]);
        var cache = CreateCache(maximumCacheEntryBytes: 3);

        await Assert.That(async () =>
                await cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>();
    }

    [Test]
    public async Task OpenReadRejectsMissingChunk()
    {
        await StoreEntryAsync($"{Generation}:2:5", [1, 2, 3]);

        await Assert.That(async () =>
                await _cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>()
            .WithMessageContaining("missing chunk 1");
    }

    [Test]
    public async Task OpenReadAcceptsEntriesWrittenWithALargerChunkSize()
    {
        // Written before ChunkSizeBytes was lowered: one five-byte chunk, read with three-byte chunks.
        await StoreEntryAsync($"{Generation}:1:5", [1, 2, 3, 4, 5]);

        await using var result = await _cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        using var destination = new MemoryStream();
        await result!.CopyToAsync(destination);
        await Assert.That(destination.ToArray()).IsEquivalentTo(new byte[] { 1, 2, 3, 4, 5 });
    }

    [Test]
    [Arguments("1000:1")]
    [Arguments("0:5")]
    [Arguments("1:0")]
    public async Task OpenReadRejectsInconsistentChunkCount(string countAndLength)
    {
        await _redis.Client.SetAsync(MetadataKey, (RespireValue) $"{Generation}:{countAndLength}");
        using var chunkReads = _redis.InjectFault("GET", ChunkKey(0), RespireFakeFault.Delay(TimeSpan.Zero), occurrences: null);

        await Assert.That(async () =>
                await _cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>();

        await Assert.That(chunkReads.MatchedCount).IsEqualTo(0);
    }

    [Test]
    public async Task OpenReadCancellationInterruptsPendingRedisCall()
    {
        var gate = new RespireFakeGate();
        using var fault = _redis.InjectFault("GET", MetadataKey, RespireFakeFault.Pause(gate));
        using var cancellationTokenSource = new CancellationTokenSource();

        var readTask = _cache.OpenReadAsync(Fingerprint, cancellationTokenSource.Token);
        await fault.Matched.WaitAsync(TimeSpan.FromSeconds(5));
        await cancellationTokenSource.CancelAsync();

        try
        {
            await Assert.That(async () => await readTask.WaitAsync(TimeSpan.FromSeconds(5)))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            gate.Release();
        }
    }

    [Test]
    public async Task CacheRegistrationDoesNotReplaceDistributedOptions()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisDistributed(options => options with
        {
            ConnectionString = "distributed:6379",
            KeyPrefix = "distributed",
            ChunkSizeBytes = 123,
        });
        builder.AddRedisModuleCache(options => options with
        {
            ConnectionString = "cache:6379",
            KeyPrefix = "cache",
            ChunkSizeBytes = 456,
        });

        using var serviceProvider = builder.Services.BuildServiceProvider();
        var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("distributed:6379");
            await Assert.That(redisOptions.KeyPrefix).IsEqualTo("distributed");
            await Assert.That(redisOptions.ChunkSizeBytes).IsEqualTo(123);
            // Neither feature registers or adopts an application-visible client.
            await Assert.That(builder.Services.Any(descriptor =>
                    descriptor.ServiceType == typeof(IRespireClient)
                    || descriptor.ServiceType == typeof(RespireClient)))
                .IsFalse();
        }
    }

    [Test]
    public async Task CacheRejectsMissingConnectionAtStartup()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisModuleCache(options => options with { KeyPrefix = "cache" });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<Microsoft.Extensions.Options.OptionsValidationException>()
            .WithMessageContaining(nameof(RedisOptions.ConnectionString));
    }

    private static string ChunkKey(int chunkIndex) => $"{FingerprintPrefix}:entry:{Generation}:chunk:{chunkIndex}";

    private async Task StoreEntryAsync(string metadata, params byte[][] chunks)
    {
        await _redis.Client.SetAsync(MetadataKey, (RespireValue) metadata);
        for (var index = 0; index < chunks.Length; index++)
        {
            await _redis.Client.SetAsync(ChunkKey(index), (RespireValue) chunks[index]);
        }
    }

    private RedisModuleCache CreateCache(long maximumCacheEntryBytes) =>
        new(
            new RedisConnectionProvider(_redis.Client),
            new RedisOptions
            {
                KeyPrefix = "custom-prefix",
                ChunkSizeBytes = 3,
                TimeToLive = TimeSpan.FromMinutes(1),
            },
            new ModuleCacheOptions { MaxCacheEntryBytes = maximumCacheEntryBytes });

    // Returns its content, then fails, like a source that breaks part way through a write.
    private sealed class FailAfterStream(byte[] content) : MemoryStream(content)
    {
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var read = await base.ReadAsync(buffer, cancellationToken);
            return read > 0 ? read : throw new IOException("Simulated read failure");
        }
    }
}
