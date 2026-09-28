using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Distributed.Redis.Coordination;
using StackExchange.Redis;
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
            connection.GetDatabase(),
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

    private static async Task<ConnectionMultiplexer> ConnectAsync()
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new SkipTestException($"Set {ConnectionStringVariable} to run real Redis storage tests.");
        }

        return await ConnectionMultiplexer.ConnectAsync(connectionString);
    }
}
