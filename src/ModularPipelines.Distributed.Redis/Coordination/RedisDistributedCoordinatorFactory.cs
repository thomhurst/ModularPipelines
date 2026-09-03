using Microsoft.Extensions.Options;
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
    private readonly DistributedOptions _distributedOptions;

    public RedisDistributedCoordinatorFactory(
        IOptions<RedisDistributedOptions> options,
        IConnectionMultiplexer connection,
        IOptions<DistributedOptions> distributedOptions)
    {
        _options = options.Value;
        _connection = connection;
        _distributedOptions = distributedOptions.Value;
    }

    public Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken)
    {
        IDistributedMasterCoordinator coordinator = CreateCoordinator();
        return Task.FromResult(coordinator);
    }

    public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken)
    {
        IDistributedWorkerCoordinator coordinator = CreateCoordinator();
        return Task.FromResult(coordinator);
    }

    private RedisDistributedCoordinator CreateCoordinator()
    {
        var database = _connection.GetDatabase();
        var subscriber = _connection.GetSubscriber();
        var runId = RunIdentifierResolver.Resolve(_options.RunIdentifier);
        var keys = new RedisKeyBuilder(_options.KeyPrefix, runId);
        return new RedisDistributedCoordinator(
            database,
            subscriber,
            keys,
            _options,
            distributedOptions: _distributedOptions);
    }
}
