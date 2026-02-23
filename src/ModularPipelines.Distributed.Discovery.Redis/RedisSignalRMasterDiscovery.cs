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
    private readonly IConnectionMultiplexer _connection;
    private readonly RedisDiscoveryOptions _options;
    private readonly ILogger<RedisSignalRMasterDiscovery> _logger;
    private readonly string _masterUrlKey;

    public RedisSignalRMasterDiscovery(
        IConnectionMultiplexer connection,
        RedisDiscoveryOptions options,
        ILogger<RedisSignalRMasterDiscovery> logger)
    {
        _connection = connection;
        _options = options;
        _logger = logger;

        var runId = RunIdentifierResolver.Resolve(options.RunIdentifier);
        _masterUrlKey = $"{options.KeyPrefix}:{runId}:master-url";
    }

    public async Task AdvertiseMasterUrlAsync(string masterUrl, CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();
        var ttl = TimeSpan.FromSeconds(_options.TtlSeconds);

        await db.StringSetAsync(_masterUrlKey, masterUrl, ttl);
        _logger.LogInformation("Advertised master URL '{Url}' to Redis key '{Key}' (TTL: {Ttl}s)",
            masterUrl, _masterUrlKey, _options.TtlSeconds);
    }

    public async Task<string> DiscoverMasterUrlAsync(CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(_options.DiscoveryTimeoutSeconds));

        _logger.LogInformation("Waiting for master URL at Redis key '{Key}'...", _masterUrlKey);

        while (!timeoutCts.IsCancellationRequested)
        {
            var value = await db.StringGetAsync(_masterUrlKey);
            if (value.HasValue)
            {
                var masterUrl = value.ToString();
                _logger.LogInformation("Discovered master URL: {Url}", masterUrl);
                return masterUrl;
            }

            await Task.Delay(_options.PollIntervalMs, timeoutCts.Token);
        }

        throw new TimeoutException(
            $"Failed to discover master URL within {_options.DiscoveryTimeoutSeconds} seconds. " +
            $"Redis key: {_masterUrlKey}");
    }
}
