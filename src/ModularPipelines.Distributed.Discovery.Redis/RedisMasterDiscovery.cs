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
/// Run cleanup deletes the endpoint this process advertised, so the token does not outlive the run.
/// </remarks>
internal sealed class RedisMasterDiscovery(
    IRedisDiscoveryStore store,
    RedisDiscoveryOptions options,
    DistributedOptions distributedOptions,
    ILogger<RedisMasterDiscovery> logger,
    TimeProvider? timeProvider = null) : IMasterDiscovery, IDistributedRunCleanup, IAsyncDisposable
{
    private readonly IRedisDiscoveryStore _store = store;
    private readonly RedisDiscoveryOptions _options = options;
    private readonly ILogger<RedisMasterDiscovery> _logger = logger;
    private readonly string _masterEndpointKey = $"{options.KeyPrefix}:{distributedOptions.RunId}:master-endpoint";
    private readonly SemaphoreSlim _lifecycleLock = new(1, 1);
    private readonly CancellationTokenSource _renewalCancellation = new();
    private readonly TimeProvider _timeProvider = timeProvider ?? TimeProvider.System;
    private Task _renewal = Task.CompletedTask;
    private string? _advertisedValue;
    private bool _stopped;

    public async Task AdvertiseMasterEndpointAsync(MasterEndpoint endpoint, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        await _lifecycleLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ObjectDisposedException.ThrowIf(_stopped, this);
            if (_advertisedValue is not null)
            {
                throw new InvalidOperationException("This discovery instance has already advertised a master endpoint.");
            }

            // Retain ownership even on an ambiguous write failure so cleanup can remove the token.
            // A unique advertisement ID distinguishes replacement masters with identical URLs and tokens.
            _advertisedValue = JsonSerializer.Serialize(new StoredEndpoint(endpoint.Url.AbsoluteUri, endpoint.AccessToken, Guid.NewGuid().ToString("N")));
            await _store.SetAsync(_masterEndpointKey, _advertisedValue, _options.TimeToLive, cancellationToken).ConfigureAwait(false);
            _renewal = RenewAsync(_advertisedValue, _renewalCancellation.Token);
            _logger.LogInformation(
                "Advertised the master endpoint in Redis (time to live: {TimeToLive})",
                _options.TimeToLive);
            _logger.LogDebug("Advertised master URL {Url} at key {Key}", endpoint.Url, _masterEndpointKey);
        }
        finally
        {
            _lifecycleLock.Release();
        }
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

    public async Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        await _lifecycleLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await StopRenewalAsync().ConfigureAwait(false);
            if (_advertisedValue is { } value)
            {
                await _store.DeleteAsync(_masterEndpointKey, value, cancellationToken).ConfigureAwait(false);
                _advertisedValue = null;
                _logger.LogDebug("Removed this master's advertised endpoint at key {Key}", _masterEndpointKey);
            }
        }
        finally
        {
            _lifecycleLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _lifecycleLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await StopRenewalAsync().ConfigureAwait(false);
        }
        finally
        {
            _lifecycleLock.Release();
        }
    }

    private async Task StopRenewalAsync()
    {
        if (_stopped)
        {
            return;
        }

        _stopped = true;
        await _renewalCancellation.CancelAsync().ConfigureAwait(false);
        await _renewal.ConfigureAwait(false);
        _renewalCancellation.Dispose();
    }

    private async Task RenewAsync(string value, CancellationToken cancellationToken)
    {
        var interval = TimeSpan.FromTicks(Math.Min(_options.TimeToLive.Ticks / 3, TimeSpan.FromMinutes(1).Ticks));
        using var timer = new PeriodicTimer(interval, _timeProvider);
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    if (!await _store.RefreshAsync(_masterEndpointKey, value, _options.TimeToLive, cancellationToken).ConfigureAwait(false))
                    {
                        _logger.LogWarning("Master endpoint renewal stopped because the advertisement expired, was removed, or was replaced");
                        return;
                    }
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    // Store exceptions can include command arguments containing the access token.
                    _logger.LogWarning("Could not renew the master endpoint ({ErrorType}); retrying at the next renewal interval", exception.GetType().Name);
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Cleanup/disposal cancels the timer and any in-flight store operation before returning.
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

    private sealed record StoredEndpoint(string Url, string? AccessToken, string? AdvertisementId = null);
}
