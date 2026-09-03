using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis.Extensions;
using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Distributed.Redis.Configuration;
using Moq;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Caching;

public class RedisModuleCacheTests
{
    private const string Fingerprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    private Mock<IDatabase> _database = null!;
    private Mock<ITransaction> _transaction = null!;
    private IConnectionMultiplexer _connection = null!;
    private RedisModuleCache _cache = null!;

    [Before(Test)]
    public void Setup()
    {
        _database = new Mock<IDatabase>();
        _transaction = new Mock<ITransaction>();
        _database.Setup(value => value.CreateTransaction(It.IsAny<object>()))
            .Returns(_transaction.Object);
        _transaction.Setup(value => value.ExecuteAsync(It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);
        _transaction.Setup(value => value.KeyExpireAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<ExpireWhen>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);
        _transaction.Setup(value => value.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);
        var connection = new Mock<IConnectionMultiplexer>();
        connection.Setup(value => value.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_database.Object);
        _connection = connection.Object;
        _cache = new RedisModuleCache(
            _connection,
            new RedisDistributedOptions
            {
                KeyPrefix = "custom-prefix",
                RunIdentifier = "must-not-appear",
            },
            new ArtifactOptions
            {
                ChunkSizeBytes = 3,
                TimeToLive = TimeSpan.FromMinutes(1),
            });
    }

    [Test]
    public async Task WriteUsesChunkedStableCrossRunKeys()
    {
        await using var content = new MemoryStream([1, 2, 3, 4, 5]);

        await _cache.WriteAsync(Fingerprint, content, CancellationToken.None);

        var databaseWrites = _database.Invocations
            .Where(invocation => invocation.Method.Name == nameof(IDatabase.StringSetAsync))
            .ToArray();
        var transactionWrites = _transaction.Invocations
            .Where(invocation => invocation.Method.Name == nameof(IDatabase.StringSetAsync))
            .ToArray();
        var keys = databaseWrites
            .Concat(transactionWrites)
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
        await Assert.That(databaseWrites.All(invocation =>
            invocation.Arguments[2].Equals(new Expiration(TimeSpan.FromHours(1))))).IsTrue();
        _transaction.Verify(value => value.KeyExpireAsync(
                It.Is<RedisKey>(key => key.ToString().Contains(":entry:", StringComparison.Ordinal)),
                TimeSpan.FromSeconds(60),
                ExpireWhen.Always,
                CommandFlags.None),
            Times.Exactly(2));
        _transaction.Verify(value => value.ExecuteAsync(CommandFlags.None), Times.Once);
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

    [Test]
    public async Task OpenReadRejectsMetadataLengthAboveConfiguredLimit()
    {
        var generation = new string('b', 32);
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:1:4");
        var cache = CreateCache(maximumCacheEntryBytes: 3);

        await Assert.That(async () =>
                await cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>();

        _database.Verify(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().Contains(":chunk:", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()),
            Times.Never);
    }

    [Test]
    public async Task OpenReadRejectsChunksAboveConfiguredLimit()
    {
        var generation = new string('b', 32);
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:2:3");
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().Contains(":chunk:", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) new byte[] { 1, 2 });
        var cache = CreateCache(maximumCacheEntryBytes: 3);

        await Assert.That(async () =>
                await cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>();
    }

    [Test]
    public async Task OpenReadRejectsInconsistentChunkCount()
    {
        var generation = new string('b', 32);
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:1000:1");

        await Assert.That(async () =>
                await _cache.OpenReadAsync(Fingerprint, CancellationToken.None))
            .Throws<InvalidDataException>();

        _database.Verify(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().Contains(":chunk:", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()),
            Times.Never);
    }

    [Test]
    public async Task OpenReadCancellationInterruptsPendingRedisCall()
    {
        var pending = new TaskCompletionSource<RedisValue>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        _database.Setup(value => value.StringGetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
            .Returns(pending.Task);
        using var cancellationTokenSource = new CancellationTokenSource();

        var readTask = _cache.OpenReadAsync(Fingerprint, cancellationTokenSource.Token);
        await cancellationTokenSource.CancelAsync();

        await Assert.That(async () => await readTask).Throws<OperationCanceledException>();
    }

    [Test]
    public async Task CacheRegistrationDoesNotReplaceDistributedOptions()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisDistributed(
            options =>
            {
                options.ConnectionString = "distributed:6379";
                options.KeyPrefix = "distributed";
                options.RunIdentifier = "distributed-run";
            },
            options => options.ChunkSizeBytes = 123);
        builder.AddRedisModuleCache(
            options =>
            {
                options.ConnectionString = "cache:6379";
                options.KeyPrefix = "cache";
            },
            options => options.ChunkSizeBytes = 456);

        using var serviceProvider = builder.Services.BuildServiceProvider();
        var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisDistributedOptions>>().Value;
        var artifactOptions = serviceProvider.GetRequiredService<IOptions<ArtifactOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("distributed:6379");
            await Assert.That(redisOptions.KeyPrefix).IsEqualTo("distributed");
            await Assert.That(artifactOptions.ChunkSizeBytes).IsEqualTo(123);
            await Assert.That(builder.Services.Count(descriptor =>
                    descriptor.ServiceType == typeof(IConnectionMultiplexer)
                    && descriptor.IsKeyedService))
                .IsEqualTo(1);
        }
    }

    private RedisModuleCache CreateCache(long maximumCacheEntryBytes) =>
        new(
            _connection,
            new RedisDistributedOptions { KeyPrefix = "custom-prefix" },
            new ArtifactOptions
            {
                ChunkSizeBytes = 3,
                TimeToLive = TimeSpan.FromMinutes(1),
            },
            new ModuleCacheOptions { MaximumCacheEntryBytes = maximumCacheEntryBytes });
}
