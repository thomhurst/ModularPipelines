using ModularPipelines.Distributed.Redis.Configuration;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedCoordinator"/> by connecting to Redis asynchronously.
/// </summary>
internal sealed class RedisDistributedCoordinatorFactory : IDistributedCoordinatorFactory
{
    private readonly RedisDistributedOptions _options;

    public RedisDistributedCoordinatorFactory(RedisDistributedOptions options)
    {
        _options = options;
    }

    public async Task<IDistributedCoordinator> CreateAsync(CancellationToken cancellationToken)
    {
        var connection = await ConnectionMultiplexer.ConnectAsync(_options.ConnectionString);
        var database = connection.GetDatabase();
        var subscriber = connection.GetSubscriber();
        var runId = RunIdentifierResolver.Resolve(_options.RunIdentifier);
        var keys = new RedisKeyBuilder(_options.KeyPrefix, runId);
        return new RedisDistributedCoordinator(database, subscriber, keys, _options);
    }
}
