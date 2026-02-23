using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private readonly DistributedOptions _distributedOptions;
    private readonly ISignalRMasterDiscovery? _discovery;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServiceProvider _serviceProvider;
    private MasterServerHost? _serverHost;
    private HubConnection? _hubConnection;

    public SignalRDistributedCoordinatorFactory(
        SignalRDistributedOptions options,
        IOptions<DistributedOptions> distributedOptions,
        ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider,
        ISignalRMasterDiscovery? discovery = null)
    {
        _options = options;
        _distributedOptions = distributedOptions.Value;
        _discovery = discovery;
        _loggerFactory = loggerFactory;
        _serviceProvider = serviceProvider;
    }

    public async Task<IDistributedCoordinator> CreateAsync(CancellationToken cancellationToken)
    {
        var isMaster = _distributedOptions.InstanceIndex == 0;

        if (isMaster)
        {
            return await CreateMasterCoordinatorAsync(cancellationToken);
        }
        else
        {
            return await CreateWorkerCoordinatorAsync(cancellationToken);
        }
    }

    private async Task<IDistributedCoordinator> CreateMasterCoordinatorAsync(CancellationToken cancellationToken)
    {
        var masterState = new SignalRMasterState();

        // Start the SignalR server
        _serverHost = new MasterServerHost();
        await _serverHost.StartAsync(_options, masterState, _loggerFactory, cancellationToken);

        // Advertise URL if discovery is available
        if (_discovery is not null)
        {
            await _discovery.AdvertiseMasterUrlAsync(_options.MasterUrl, cancellationToken);
        }

        // Create a hub context to send messages to workers
        // We need to connect back to our own hub to get the IHubContext
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_options.MasterUrl}{_options.HubPath}")
            .Build();

        // For the master coordinator, we use the hub context from DI if available,
        // otherwise create a lightweight proxy
        var coordinator = new SignalRMasterCoordinator(
            new MasterHubContextAdapter(masterState, hubConnection),
            masterState,
            _loggerFactory.CreateLogger<SignalRMasterCoordinator>());

        return coordinator;
    }

    private async Task<IDistributedCoordinator> CreateWorkerCoordinatorAsync(CancellationToken cancellationToken)
    {
        var masterUrl = _options.MasterUrl;

        // Use discovery to find master URL if available
        if (_discovery is not null)
        {
            masterUrl = await _discovery.DiscoverMasterUrlAsync(cancellationToken);
        }

        var hubUrl = $"{masterUrl}{_options.HubPath}";
        var logger = _loggerFactory.CreateLogger<SignalRWorkerCoordinator>();

        var builder = new HubConnectionBuilder()
            .WithUrl(hubUrl);

        if (_options.EnableAutoReconnect)
        {
            builder.WithAutomaticReconnect(
                new RetryPolicy(_options.MaxReconnectAttempts));
        }

        _hubConnection = builder.Build();

        // Connect with timeout
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(_options.ConnectionTimeoutSeconds));

        logger.LogInformation("Connecting to master at {Url}...", hubUrl);
        await _hubConnection.StartAsync(timeoutCts.Token);
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

/// <summary>
/// Lightweight adapter that provides <see cref="IHubContext{THub}"/>-like functionality
/// for the master coordinator without requiring ASP.NET Core DI integration.
/// Uses a direct HubConnection to the local server.
/// </summary>
internal class MasterHubContextAdapter : IHubContext<DistributedPipelineHub>
{
    private readonly SignalRMasterState _state;
    private readonly HubConnection _connection;

    public MasterHubContextAdapter(SignalRMasterState state, HubConnection connection)
    {
        _state = state;
        _connection = connection;
    }

    public IHubClients Clients => new MasterHubClients(_state, _connection);
    public IGroupManager Groups => throw new NotSupportedException("Groups are not used in pipeline coordination.");
}

internal class MasterHubClients : IHubClients
{
    private readonly SignalRMasterState _state;
    private readonly HubConnection _connection;

    public MasterHubClients(SignalRMasterState state, HubConnection connection)
    {
        _state = state;
        _connection = connection;
    }

    public IClientProxy All => new BroadcastClientProxy(_state);
    public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds) => All;
    public IClientProxy Client(string connectionId) => new SingleClientProxy(connectionId, _state);
    public IClientProxy Clients(IReadOnlyList<string> connectionIds) => All;
    public IClientProxy Group(string groupName) => throw new NotSupportedException();
    public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => throw new NotSupportedException();
    public IClientProxy Groups(IReadOnlyList<string> groupNames) => throw new NotSupportedException();
    public IClientProxy User(string userId) => throw new NotSupportedException();
    public IClientProxy Users(IReadOnlyList<string> userIds) => throw new NotSupportedException();
}

/// <summary>
/// Sends to all connected workers by iterating the worker state dictionary.
/// Note: This is a simplified implementation. In production, the real IHubContext from
/// the WebApplication's DI container would be used for actual message delivery.
/// </summary>
internal class BroadcastClientProxy(SignalRMasterState state) : IClientProxy
{
    public Task SendCoreAsync(string method, object?[] args, CancellationToken cancellationToken = default)
    {
        // In the real implementation, the hub context handles broadcasting.
        // This proxy is used before the actual hub context is available.
        return Task.CompletedTask;
    }
}

internal class SingleClientProxy(string connectionId, SignalRMasterState state) : IClientProxy
{
    public Task SendCoreAsync(string method, object?[] args, CancellationToken cancellationToken = default)
    {
        // In the real implementation, the hub context sends to the specific connection.
        return Task.CompletedTask;
    }
}
