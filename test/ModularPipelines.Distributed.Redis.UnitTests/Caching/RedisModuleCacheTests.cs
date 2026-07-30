using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Distributed.Redis.Configuration;
using Moq;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Caching;

public class RedisModuleCacheTests
{
    private const string Fingerprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    private Mock<IDatabase> _database = null!;
    private RedisModuleCache _cache = null!;

    [Before(Test)]
    public void Setup()
    {
        _database = new Mock<IDatabase>();
        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(value => value.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_database.Object);
        _cache = new RedisModuleCache(
            connection.Object,
            new RedisDistributedOptions
            {
                KeyPrefix = "custom-prefix",
                RunIdentifier = "must-not-appear",
            },
            new ArtifactOptions
            {
                ChunkSizeBytes = 3,
                TimeToLiveSeconds = 60,
            });
    }

    [Test]
    public async Task WriteUsesChunkedStableCrossRunKeys()
    {
        await using var content = new MemoryStream([1, 2, 3, 4, 5]);

        await _cache.WriteAsync(Fingerprint, content, CancellationToken.None);

        var keys = _database.Invocations
            .Where(invocation => invocation.Method.Name == nameof(IDatabase.StringSetAsync))
            .Select(invocation => invocation.Arguments[0].ToString()!)
            .ToList();
        var fingerprintPrefix = $"custom-prefix:module-cache:v1:{Fingerprint.ToLowerInvariant()}";
        var entryKeys = keys
            .Where(key => key.StartsWith($"{fingerprintPrefix}:entry:", StringComparison.Ordinal))
            .ToArray();
        await Assert.That(keys.Count).IsEqualTo(3);
        await Assert.That(entryKeys.Length).IsEqualTo(2);
        await Assert.That(entryKeys.Select(key => key[..key.LastIndexOf(":chunk:", StringComparison.Ordinal)]).Distinct().Count()).IsEqualTo(1);
        await Assert.That(keys).Contains($"{fingerprintPrefix}:metadata");
        await Assert.That(keys.Any(key => key.EndsWith(":chunk:0", StringComparison.Ordinal))).IsTrue();
        await Assert.That(keys.Any(key => key.EndsWith(":chunk:1", StringComparison.Ordinal))).IsTrue();
        await Assert.That(keys.Any(key => key.Contains("must-not-appear", StringComparison.Ordinal))).IsFalse();
    }

    [Test]
    public async Task OpenReadReassemblesChunks()
    {
        var generation = new string('b', 32);
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:2:5");
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":chunk:0", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) new byte[] { 1, 2, 3 });
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":chunk:1", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) new byte[] { 4, 5 });

        await using var result = await _cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        using var destination = new MemoryStream();
        await result!.CopyToAsync(destination);
        await Assert.That(destination.ToArray()).IsEquivalentTo(new byte[] { 1, 2, 3, 4, 5 });
    }

    [Test]
    public async Task OpenReadReturnsNullWhenMetadataIsMissing()
    {
        _database.Setup(value => value.StringGetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        var result = await _cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNull();
    }
}
