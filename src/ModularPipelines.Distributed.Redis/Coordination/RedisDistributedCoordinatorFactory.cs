using ModularPipelines.Distributed.Redis.Configuration;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedCoordinator"/> by connecting to Redis asynchronously.
/// </summary>
internal sealed class RedisDistributedCoordinatorFactory : IDistributedCoordinatorFactory
{
    private readonly RedisDistributedOptions _options;
    private readonly IConnectionMultiplexer _connection;

    public RedisDistributedCoordinatorFactory(RedisDistributedOptions options, IConnectionMultiplexer connection)
    {
        _options = options;
        _connection = connection;
    }

    public Task<IDistributedCoordinator> CreateAsync(CancellationToken cancellationToken)
    {
        var database = _connection.GetDatabase();
        var subscriber = _connection.GetSubscriber();
        var runId = RunIdentifierResolver.Resolve(_options.RunIdentifier);
        var keys = new RedisKeyBuilder(_options.KeyPrefix, runId);
        IDistributedCoordinator coordinator = new RedisDistributedCoordinator(database, subscriber, keys, _options);
        return Task.FromResult(coordinator);
    }
}
