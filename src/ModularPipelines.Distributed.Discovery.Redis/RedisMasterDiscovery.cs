using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Redis-based implementation of <see cref="IMasterDiscovery"/>.
/// The master writes its endpoint to Redis; workers poll until they find it.
/// </summary>
/// <remarks>
/// The stored endpoint can include the master's access token, so the key must only be readable by
/// the run's processes. Neither the URL nor the token is logged above Debug, and the token never is.
/// </remarks>
internal sealed class RedisMasterDiscovery(
    IRedisDiscoveryStore store,
    RedisDiscoveryOptions options,
    DistributedOptions distributedOptions,
    ILogger<RedisMasterDiscovery> logger) : IMasterDiscovery
{
    private readonly IRedisDiscoveryStore _store = store;
    private readonly RedisDiscoveryOptions _options = options;
    private readonly ILogger<RedisMasterDiscovery> _logger = logger;
    private readonly string _masterEndpointKey = $"{options.KeyPrefix}:{distributedOptions.RunId}:master-endpoint";

    public async Task AdvertiseMasterEndpointAsync(MasterEndpoint endpoint, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        var value = JsonSerializer.Serialize(new StoredEndpoint(endpoint.Url.AbsoluteUri, endpoint.AccessToken));
        await _store.SetAsync(_masterEndpointKey, value, _options.TimeToLive, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation(
            "Advertised the master endpoint in Redis (time to live: {TimeToLive})",
            _options.TimeToLive);
        _logger.LogDebug("Advertised master URL {Url} at key {Key}", endpoint.Url, _masterEndpointKey);
    }

    public async Task<MasterEndpoint> DiscoverMasterEndpointAsync(CancellationToken cancellationToken)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(_options.DiscoveryTimeout);

        _logger.LogInformation("Waiting for the master endpoint in Redis...");

        try
        {
            while (true)
            {
                var value = await _store.GetAsync(_masterEndpointKey, timeoutCts.Token).ConfigureAwait(false);
                if (value is not null)
                {
                    var endpoint = Parse(value);
                    _logger.LogInformation("Discovered the master endpoint");
                    _logger.LogDebug("Discovered master URL {Url}", endpoint.Url);
                    return endpoint;
                }

                await Task.Delay(_options.PollInterval, timeoutCts.Token).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException exception)
            when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
        {
            // Only an elapsed discovery timeout is translated; caller cancellation and a store
            // that cancels on its own propagate as-is.
            throw new TimeoutException(
                $"Failed to discover master endpoint within {_options.DiscoveryTimeout}. " +
                $"Redis key: {_masterEndpointKey}",
                exception);
        }
    }

    private MasterEndpoint Parse(string value)
    {
        StoredEndpoint? stored;
        try
        {
            stored = JsonSerializer.Deserialize<StoredEndpoint>(value);
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException(
                $"The master endpoint stored at Redis key '{_masterEndpointKey}' is not valid.",
                exception);
        }

        if (stored is null || !Uri.TryCreate(stored.Url, UriKind.Absolute, out var url))
        {
            throw new InvalidOperationException(
                $"The master endpoint stored at Redis key '{_masterEndpointKey}' has no valid URL.");
        }

        return new MasterEndpoint { Url = url, AccessToken = stored.AccessToken };
    }

    private sealed record StoredEndpoint(string Url, string? AccessToken);
}
