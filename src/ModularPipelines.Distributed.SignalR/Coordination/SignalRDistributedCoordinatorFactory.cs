using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Creates the SignalR master (which hosts the hub) or a worker connection to it.
/// </summary>
internal sealed class SignalRDistributedCoordinatorFactory(
    IOptions<SignalRDistributedOptions> options,
    IOptions<DistributedOptions> distributedOptions,
    ILoggerFactory loggerFactory,
    IMasterDiscovery? discovery = null) : IDistributedCoordinatorFactory
{
    private readonly SignalRDistributedOptions _options = options.Value;
    private readonly DistributedOptions _distributedOptions = distributedOptions.Value;

    public async Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken)
    {
        var accessToken = ResolveMasterAccessToken(_options, discovery is not null);
        var coordinator = new InMemoryDistributedCoordinator(distributedOptions);
        var masterState = new SignalRMasterState(coordinator, _distributedOptions.RunId);

        var serverHost = new MasterServerHost();
        try
        {
            await serverHost.StartAsync(_options, masterState, accessToken, loggerFactory, cancellationToken)
                .ConfigureAwait(false);

            if (discovery is not null)
            {
                await discovery.AdvertiseMasterEndpointAsync(
                        new MasterEndpoint { Url = serverHost.AdvertisedUrl, AccessToken = accessToken },
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch
        {
            await serverHost.DisposeAsync().ConfigureAwait(false);
            throw;
        }

        return new SignalRMasterCoordinator(coordinator, serverHost);
    }

    public async Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken)
    {
        var endpoint = discovery is not null
            ? await discovery.DiscoverMasterEndpointAsync(cancellationToken).ConfigureAwait(false)
            : new MasterEndpoint
            {
                Url = _options.AdvertisedUrl ?? _options.ListenUrl,
                AccessToken = _options.AccessToken,
            };
        var accessToken = endpoint.AccessToken ?? _options.AccessToken;
        var hubUrl = new Uri(endpoint.Url, _options.HubPath);
        var logger = loggerFactory.CreateLogger<SignalRWorkerCoordinator>();

        // Connect with timeout and retry (DNS for tunnel URLs may need propagation time).
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(_options.ConnectionTimeout);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Connecting to master at {Url}", hubUrl);
        }

        var attempt = 0;
        while (true)
        {
            var connection = BuildHubConnection(hubUrl, accessToken);
            try
            {
                await connection.StartAsync(timeoutCts.Token).ConfigureAwait(false);
                logger.LogInformation("Connected to the distributed master");
                return new SignalRWorkerCoordinator(connection, logger);
            }
            catch (Exception ex) when (!timeoutCts.IsCancellationRequested)
            {
                attempt++;
                logger.LogWarning("Connection attempt {Attempt} failed: {Error}. Retrying...", attempt, ex.Message);
                await connection.DisposeAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromSeconds(Math.Min(attempt * 2, 10)), timeoutCts.Token).ConfigureAwait(false);
            }
            catch
            {
                await connection.DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }
    }

    /// <summary>
    /// Returns the configured token, or generates one when the master is reachable beyond this
    /// machine. A generated token can only reach workers through discovery.
    /// </summary>
    internal static string? ResolveMasterAccessToken(SignalRDistributedOptions options, bool hasDiscovery)
    {
        if (options.AccessToken is { } configured)
        {
            return configured;
        }

        // A loopback listener behind a reverse proxy is still reachable through its advertised URL.
        if (!options.Tunnel.Enabled
            && IsLoopback(options.ListenUrl)
            && (options.AdvertisedUrl is null || IsLoopback(options.AdvertisedUrl)))
        {
            return null;
        }

        if (!hasDiscovery)
        {
            throw new InvalidOperationException(
                "The SignalR master is reachable beyond this machine but has no access token. Configure "
                + $"{nameof(SignalRDistributedOptions)}.{nameof(SignalRDistributedOptions.AccessToken)} on every process, "
                + "or register master discovery so the master can share a generated token with its workers.");
        }

        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    internal static bool IsLoopback(Uri uri)
    {
        if (!uri.IsAbsoluteUri)
        {
            return false;
        }

        return uri.IsLoopback
               || (IPAddress.TryParse(uri.Host.Trim('[', ']'), out var address) && IPAddress.IsLoopback(address));
    }

    private HubConnection BuildHubConnection(Uri hubUrl, string? accessToken)
    {
        var builder = new HubConnectionBuilder()
            .WithUrl(hubUrl, connectionOptions =>
            {
                if (accessToken is not null)
                {
                    connectionOptions.AccessTokenProvider = () => Task.FromResult<string?>(accessToken);
                }
            })
            .AddJsonProtocol(jsonOptions =>
            {
                // Match server-side settings: PascalCase, case-insensitive
                jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        if (_options.MaxReconnectAttempts > 0)
        {
            builder.WithAutomaticReconnect(new RetryPolicy(_options.MaxReconnectAttempts));
        }

        var connection = builder.Build();

        // Ping the master frequently and give up on it quickly, so a dead master is detected fast
        // (triggering reconnect) and the master detects a dead worker fast.
        connection.KeepAliveInterval = _options.KeepAliveInterval;
        connection.ServerTimeout = _options.PeerTimeout;
        return connection;
    }

    /// <summary>
    /// Retry policy with configurable max attempts.
    /// </summary>
    private sealed class RetryPolicy(int maxAttempts) : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            if (retryContext.PreviousRetryCount >= maxAttempts)
            {
                return null; // Stop retrying
            }

            // Exponential backoff: 1s, 2s, 4s, 8s, 16s...
            var delay = TimeSpan.FromSeconds(Math.Pow(2, retryContext.PreviousRetryCount));
            return delay.TotalSeconds > 30 ? TimeSpan.FromSeconds(30) : delay;
        }
    }
}
