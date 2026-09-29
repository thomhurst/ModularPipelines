using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Caching;
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
        _cache = CreateCache(maximumCacheEntryBytes: new ModuleCacheOptions().MaximumCacheEntryBytes);
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
            .Select(invocation => invocation.Arguments[0]!.ToString()!)
            .ToList();
        // The fingerprint is a hash tag, so all of an entry's keys map to one Redis Cluster slot.
        var fingerprintPrefix = $"custom-prefix:module-cache:v2:{{{Fingerprint.ToLowerInvariant()}}}";
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
            Equals(invocation.Arguments[2], new Expiration(TimeSpan.FromHours(1))))).IsTrue();
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
    public async Task DeleteRemovesMetadataThenCurrentGenerationChunks()
    {
        var generation = new string('b', 32);
        var deletedKeys = new List<string>();
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:2:5");
        _database.Setup(value => value.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .Callback<RedisKey, CommandFlags>((key, _) => deletedKeys.Add(key.ToString()))
            .ReturnsAsync(true);
        _database.Setup(value => value.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .Callback<RedisKey[], CommandFlags>((keys, _) => deletedKeys.AddRange(keys.Select(key => key.ToString())))
            .ReturnsAsync(2);

        await _cache.DeleteAsync(Fingerprint, CancellationToken.None);

        var fingerprintPrefix = $"custom-prefix:module-cache:v2:{{{Fingerprint.ToLowerInvariant()}}}";
        await Assert.That(deletedKeys).IsEquivalentTo(new[]
        {
            $"{fingerprintPrefix}:metadata",
            $"{fingerprintPrefix}:entry:{generation}:chunk:0",
            $"{fingerprintPrefix}:entry:{generation}:chunk:1",
        });
        await Assert.That(deletedKeys[0]).IsEqualTo($"{fingerprintPrefix}:metadata");
    }

    [Test]
    public async Task DeleteIgnoresMissingEntryAndExistsChecksMetadata()
    {
        _database.Setup(value => value.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);
        _database.Setup(value => value.KeyExistsAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        await _cache.DeleteAsync(Fingerprint, CancellationToken.None);

        _database.Verify(
            value => value.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()),
            Times.Never);
        await Assert.That(await _cache.ExistsAsync(Fingerprint, CancellationToken.None)).IsTrue();
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
    public async Task OpenReadAcceptsEntriesWrittenWithALargerChunkSize()
    {
        // Written before ChunkSizeBytes was lowered: one five-byte chunk, read with three-byte chunks.
        var generation = new string('b', 32);
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:1:5");
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":chunk:0", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) new byte[] { 1, 2, 3, 4, 5 });

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
        var generation = new string('b', 32);
        _database.Setup(value => value.StringGetAsync(
                It.Is<RedisKey>(key => key.ToString().EndsWith(":metadata", StringComparison.Ordinal)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue) $"{generation}:{countAndLength}");

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
        builder.AddRedisDistributed(options =>
        {
            options.ConnectionString = "distributed:6379";
            options.KeyPrefix = "distributed";
            options.ChunkSizeBytes = 123;
        });
        builder.AddRedisModuleCache(options =>
        {
            options.ConnectionString = "cache:6379";
            options.KeyPrefix = "cache";
            options.ChunkSizeBytes = 456;
        });

        using var serviceProvider = builder.Services.BuildServiceProvider();
        var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("distributed:6379");
            await Assert.That(redisOptions.KeyPrefix).IsEqualTo("distributed");
            await Assert.That(redisOptions.ChunkSizeBytes).IsEqualTo(123);
            // Neither feature registers or adopts an application-visible multiplexer.
            await Assert.That(builder.Services.Any(descriptor =>
                    descriptor.ServiceType == typeof(IConnectionMultiplexer)))
                .IsFalse();
        }
    }

    [Test]
    public async Task CacheRejectsMissingConnectionAtStartup()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisModuleCache(options => options.KeyPrefix = "cache");

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<Microsoft.Extensions.Options.OptionsValidationException>()
            .WithMessageContaining(nameof(RedisOptions.ConnectionString));
    }

    private RedisModuleCache CreateCache(long maximumCacheEntryBytes) =>
        new(
            new RedisConnectionProvider(_connection),
            new RedisOptions
            {
                KeyPrefix = "custom-prefix",
                ChunkSizeBytes = 3,
                TimeToLive = TimeSpan.FromMinutes(1),
            },
            new ModuleCacheOptions { MaximumCacheEntryBytes = maximumCacheEntryBytes });
}
