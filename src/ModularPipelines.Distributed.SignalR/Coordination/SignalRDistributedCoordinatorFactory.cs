using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.SignalR.Configuration;
using ModularPipelines.Distributed.SignalR.Discovery;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Factory that creates the appropriate SignalR coordinator based on the detected role (master vs worker).
/// </summary>
internal class SignalRDistributedCoordinatorFactory : IDistributedCoordinatorFactory, IAsyncDisposable
{
    private readonly SignalRDistributedOptions _options;
    private readonly ISignalRMasterDiscovery? _discovery;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServiceProvider _serviceProvider;
    private MasterServerHost? _serverHost;
    private HubConnection? _hubConnection;

    public SignalRDistributedCoordinatorFactory(
        IOptions<SignalRDistributedOptions> options,
        ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider,
        ISignalRMasterDiscovery? discovery = null)
    {
        _options = options.Value;
        _discovery = discovery;
        _loggerFactory = loggerFactory;
        _serviceProvider = serviceProvider;
    }

    public async Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken)
    {
        var masterState = new SignalRMasterState
        {
            ReconnectGracePeriod = TimeSpan.FromSeconds(_options.ReconnectGraceSeconds),
            WorkerTimeout = _serviceProvider
                .GetRequiredService<IOptions<DistributedOptions>>()
                .Value.WorkerTimeout,
        };

        // Start the SignalR server
        _serverHost = new MasterServerHost();
        await _serverHost.StartAsync(_options, masterState, _loggerFactory, cancellationToken);

        // Advertise the reachable URL (tunnel URL if active, otherwise local)
        if (_discovery is not null)
        {
            await _discovery.AdvertiseMasterUrlAsync(_serverHost.AdvertisedUrl, cancellationToken);
        }

        // Use the real IHubContext from the WebApplication's DI container
        var coordinator = new SignalRMasterCoordinator(
            _serverHost.HubContext,
            masterState,
            _loggerFactory.CreateLogger<SignalRMasterCoordinator>());

        return coordinator;
    }

    public async Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken)
    {
        var masterUrl = _options.MasterUrl;

        // Use discovery to find master URL if available
        if (_discovery is not null)
        {
            masterUrl = await _discovery.DiscoverMasterUrlAsync(cancellationToken);
        }

        var hubUrl = $"{masterUrl}{_options.HubPath}";
        var logger = _loggerFactory.CreateLogger<SignalRWorkerCoordinator>();

        // Connect with timeout and retry (DNS for tunnel URLs may need propagation time)
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(_options.ConnectionTimeoutSeconds));

        logger.LogInformation("Connecting to master at {Url}...", hubUrl);

        var attempt = 0;
        while (true)
        {
            _hubConnection = BuildHubConnection(hubUrl);

            try
            {
                await _hubConnection.StartAsync(timeoutCts.Token);
                break;
            }
            catch (Exception ex) when (!timeoutCts.IsCancellationRequested)
            {
                attempt++;
                logger.LogWarning("Connection attempt {Attempt} failed: {Error}. Retrying...", attempt, ex.Message);
                await _hubConnection.DisposeAsync();
                await Task.Delay(TimeSpan.FromSeconds(Math.Min(attempt * 2, 10)), timeoutCts.Token);
            }
        }

        logger.LogInformation("Connected to master at {Url}", hubUrl);

        return new SignalRWorkerCoordinator(_hubConnection, logger);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        if (_serverHost is not null)
        {
            await _serverHost.DisposeAsync();
        }
    }

    private HubConnection BuildHubConnection(string hubUrl)
    {
        var builder = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .AddJsonProtocol(jsonOptions =>
            {
                // Match server-side settings: PascalCase, case-insensitive
                jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                jsonOptions.PayloadSerializerOptions.Converters.Add(new ReadOnlySetJsonConverter());
            });

        if (_options.EnableAutoReconnect)
        {
            builder.WithAutomaticReconnect(
                new RetryPolicy(_options.MaxReconnectAttempts));
        }

        var connection = builder.Build();

        // Ping the master frequently and give up on it quickly, so a dead master is
        // detected fast (triggering reconnect) and the master in turn detects a dead
        // worker fast (via the frequent pings) and re-queues its in-flight work.
        connection.KeepAliveInterval = TimeSpan.FromSeconds(_options.KeepAliveIntervalSeconds);
        connection.ServerTimeout = TimeSpan.FromSeconds(_options.PeerTimeoutSeconds);

        return connection;
    }

    /// <summary>
    /// Retry policy with configurable max attempts.
    /// </summary>
    private class RetryPolicy(int maxAttempts) : IRetryPolicy
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
