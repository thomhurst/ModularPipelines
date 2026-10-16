using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Factory that creates a <see cref="RedisDistributedCoordinator"/> by connecting to Redis asynchronously.
/// </summary>
internal sealed class RedisDistributedCoordinatorFactory(
    IOptions<RedisOptions> options,
    RedisConnectionProvider connections,
    IOptions<DistributedOptions> distributedOptions,
    ILogger<RedisDistributedCoordinator>? logger = null) : IDistributedCoordinatorFactory
{
    private readonly RedisOptions _options = options.Value;
    private readonly DistributedOptions _distributedOptions = distributedOptions.Value;
    private readonly RedisConnectionProvider _connections = connections;
    private readonly ILogger<RedisDistributedCoordinator>? _logger = logger;

    public async Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken)
    {
        var coordinator = await CreateCoordinatorAsync(cancellationToken).ConfigureAwait(false);
        coordinator.StartMasterHeartbeat(_distributedOptions.WorkerHeartbeatInterval);
        return coordinator;
    }

    public async Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
        await CreateCoordinatorAsync(cancellationToken).ConfigureAwait(false);

    private async Task<RedisDistributedCoordinator> CreateCoordinatorAsync(CancellationToken cancellationToken)
    {
        var connection = await _connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var keys = new RedisKeyBuilder(_options.KeyPrefix, _distributedOptions.RunId);
        await RedisDistributedCoordinator.EnsureServerSupportAsync(connection, keys, cancellationToken).ConfigureAwait(false);
        await RedisMemoryDiagnostics.CheckMemoryPolicyAsync(connection, _options, _logger, cancellationToken).ConfigureAwait(false);
        return new RedisDistributedCoordinator(
            connection,
            keys,
            _options,
            distributedOptions: _distributedOptions,
            logger: _logger);
    }
}
