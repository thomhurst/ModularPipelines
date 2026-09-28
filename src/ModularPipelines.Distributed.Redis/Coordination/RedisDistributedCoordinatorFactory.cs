using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedCoordinator"/> by connecting to Redis asynchronously.
/// </summary>
internal sealed class RedisDistributedCoordinatorFactory : IDistributedCoordinatorFactory
{
    private readonly RedisOptions _options;
    private readonly DistributedOptions _distributedOptions;
    private readonly RedisConnectionProvider _connections;

    public RedisDistributedCoordinatorFactory(
        IOptions<RedisOptions> options,
        RedisConnectionProvider connections,
        IOptions<DistributedOptions> distributedOptions)
    {
        _options = options.Value;
        _connections = connections;
        _distributedOptions = distributedOptions.Value;
    }

    public async Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken) =>
        await CreateCoordinatorAsync(cancellationToken).ConfigureAwait(false);

    public async Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
        await CreateCoordinatorAsync(cancellationToken).ConfigureAwait(false);

    private async Task<RedisDistributedCoordinator> CreateCoordinatorAsync(CancellationToken cancellationToken)
    {
        var connection = await _connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var database = connection.GetDatabase();
        var subscriber = connection.GetSubscriber();
        var keys = new RedisKeyBuilder(_options.KeyPrefix, _distributedOptions.RunId);
        return new RedisDistributedCoordinator(
            database,
            subscriber,
            keys,
            _options,
            distributedOptions: _distributedOptions);
    }
}
