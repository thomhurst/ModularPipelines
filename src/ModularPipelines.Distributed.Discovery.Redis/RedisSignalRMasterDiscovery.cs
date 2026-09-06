using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Discovery;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Redis-based implementation of <see cref="ISignalRMasterDiscovery"/>.
/// Master writes its URL to Redis; workers poll until they find it.
/// </summary>
internal class RedisSignalRMasterDiscovery : ISignalRMasterDiscovery
{
    private readonly IRedisDiscoveryStore _store;
    private readonly RedisDiscoveryOptions _options;
    private readonly ILogger<RedisSignalRMasterDiscovery> _logger;
    private readonly string _masterUrlKey;

    public RedisSignalRMasterDiscovery(
        IConnectionMultiplexer connection,
        RedisDiscoveryOptions options,
        DistributedOptions distributedOptions,
        ILogger<RedisSignalRMasterDiscovery> logger)
        : this(new StackExchangeRedisDiscoveryStore(connection), options, distributedOptions, logger)
    {
    }

    internal RedisSignalRMasterDiscovery(
        IRedisDiscoveryStore store,
        RedisDiscoveryOptions options,
        DistributedOptions distributedOptions,
        ILogger<RedisSignalRMasterDiscovery> logger)
    {
        _store = store;
        _options = options;
        _logger = logger;

        _masterUrlKey = $"{options.KeyPrefix}:{distributedOptions.RunId}:master-url";
    }

    public async Task AdvertiseMasterUrlAsync(string masterUrl, CancellationToken cancellationToken)
    {
        await _store.SetAsync(_masterUrlKey, masterUrl, _options.Ttl, cancellationToken);
        _logger.LogInformation("Advertised master URL '{Url}' to Redis key '{Key}' (TTL: {Ttl})",
            masterUrl, _masterUrlKey, _options.Ttl);
    }

    public async Task<string> DiscoverMasterUrlAsync(CancellationToken cancellationToken)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(_options.DiscoveryTimeout);

        _logger.LogInformation("Waiting for master URL at Redis key '{Key}'...", _masterUrlKey);

        while (!timeoutCts.IsCancellationRequested)
        {
            var masterUrl = await _store.GetAsync(_masterUrlKey, timeoutCts.Token);
            if (masterUrl is not null)
            {
                _logger.LogInformation("Discovered master URL: {Url}", masterUrl);
                return masterUrl;
            }

            await Task.Delay(_options.PollInterval, timeoutCts.Token);
        }

        throw new TimeoutException(
            $"Failed to discover master URL within {_options.DiscoveryTimeout}. " +
            $"Redis key: {_masterUrlKey}");
    }
}
