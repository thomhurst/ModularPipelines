using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Redis-based implementation of <see cref="IMasterDiscovery"/>.
/// The master writes its endpoint to Redis; workers poll until they find it.
/// </summary>
internal class RedisMasterDiscovery : IMasterDiscovery
{
    private readonly IRedisDiscoveryStore _store;
    private readonly RedisDiscoveryOptions _options;
    private readonly ILogger<RedisMasterDiscovery> _logger;
    private readonly string _masterEndpointKey;

    public RedisMasterDiscovery(
        IConnectionMultiplexer connection,
        RedisDiscoveryOptions options,
        DistributedOptions distributedOptions,
        ILogger<RedisMasterDiscovery> logger)
        : this(new StackExchangeRedisDiscoveryStore(connection), options, distributedOptions, logger)
    {
    }

    internal RedisMasterDiscovery(
        IRedisDiscoveryStore store,
        RedisDiscoveryOptions options,
        DistributedOptions distributedOptions,
        ILogger<RedisMasterDiscovery> logger)
    {
        _store = store;
        _options = options;
        _logger = logger;

        _masterEndpointKey = $"{options.KeyPrefix}:{distributedOptions.RunId}:master-url";
    }

    public async Task AdvertiseMasterEndpointAsync(string masterEndpoint, CancellationToken cancellationToken)
    {
        var ttl = TimeSpan.FromSeconds(_options.TtlSeconds);

        await _store.SetAsync(_masterEndpointKey, masterEndpoint, ttl, cancellationToken);
        _logger.LogInformation("Advertised master endpoint '{Endpoint}' to Redis key '{Key}' (TTL: {Ttl}s)",
            masterEndpoint, _masterEndpointKey, _options.TtlSeconds);
    }

    public async Task<string> DiscoverMasterEndpointAsync(CancellationToken cancellationToken)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(_options.DiscoveryTimeoutSeconds));

        _logger.LogInformation("Waiting for master endpoint at Redis key '{Key}'...", _masterEndpointKey);

        while (!timeoutCts.IsCancellationRequested)
        {
            var masterEndpoint = await _store.GetAsync(_masterEndpointKey, timeoutCts.Token);
            if (masterEndpoint is not null)
            {
                _logger.LogInformation("Discovered master endpoint: {Endpoint}", masterEndpoint);
                return masterEndpoint;
            }

            await Task.Delay(_options.PollIntervalMs, timeoutCts.Token);
        }

        throw new TimeoutException(
            $"Failed to discover master endpoint within {_options.DiscoveryTimeoutSeconds} seconds. " +
            $"Redis key: {_masterEndpointKey}");
    }
}
