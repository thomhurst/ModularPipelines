using System.Net.Sockets;
using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Worker-side <see cref="IDistributedWorkerCoordinator"/> backed by a SignalR <see cref="HubConnection"/>.
/// </summary>
/// <remarks>
/// Every operation is a worker-to-master invocation, so nothing the master sends can be missed while
/// the worker is disconnected. After an automatic reconnect the worker registers again under the
/// same session and retries interrupted invocations; its heartbeats then renew the leases it still
/// holds. When the connection closes for good, <see cref="DequeueModuleAsync"/> returns
/// <see langword="null"/> so the worker stops, and other operations fail.
/// </remarks>
internal sealed class SignalRWorkerCoordinator : IDistributedWorkerCoordinator, IAsyncDisposable
{
    private readonly HubConnection _connection;
    private readonly ILogger<SignalRWorkerCoordinator> _logger;

    private readonly Lock _stateLock = new();
    private TaskCompletionSource<bool> _ready = CreateReadySignal(completed: true);
    private TaskCompletionSource _changed = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private WorkerRegistration? _registration;
    private long _generation;

    public SignalRWorkerCoordinator(HubConnection connection, ILogger<SignalRWorkerCoordinator> logger)
    {
        _connection = connection;
        _logger = logger;
        _connection.Reconnecting += OnReconnectingAsync;
        _connection.Reconnected += OnReconnectedAsync;
        _connection.Closed += OnClosedAsync;
        RegisterAgainAsync = registration => _connection.InvokeAsync(HubMethodNames.RegisterWorker, registration);
    }

    /// <summary>
    /// Gets or sets how long a transport failure waits for the connection-change callback that
    /// usually follows it.
    /// </summary>
    internal TimeSpan ConnectionChangeGracePeriod { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets how the worker registers again after an automatic reconnect.
    /// </summary>
    internal Func<WorkerRegistration, Task> RegisterAgainAsync { get; set; }

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        Volatile.Write(ref _registration, registration);
        await InvokeAsync(
                token => _connection.InvokeAsync(HubMethodNames.RegisterWorker, registration, token),
                cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken)
    {
        try
        {
            return await InvokeAsync(
                    token => _connection.InvokeAsync<ModuleLease?>(HubMethodNames.DequeueModule, token),
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (MasterConnectionClosedException)
        {
            // The master is gone; there is no more work this worker can claim.
            _logger.LogWarning("Connection to the master closed; the worker stops claiming work");
            return null;
        }
    }

    public Task PublishResultAsync(SerializedModuleResult result, ModuleLease? lease, CancellationToken cancellationToken) =>
        InvokeAsync(
            token => _connection.InvokeAsync(HubMethodNames.PublishResult, result, lease, token),
            cancellationToken);

    public Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        InvokeAsync(
            token => _connection.InvokeAsync<SerializedModuleResult>(HubMethodNames.WaitForResult, moduleId, token),
            cancellationToken);

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        InvokeAsync(
            token => _connection.InvokeAsync(HubMethodNames.Heartbeat, status, token),
            cancellationToken);

    public Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken) =>
        InvokeAsync(
            token => _connection.InvokeAsync<DistributedCancellationReason>(HubMethodNames.WaitForCancellation, token),
            cancellationToken);

    public async ValueTask DisposeAsync()
    {
        _connection.Reconnecting -= OnReconnectingAsync;
        _connection.Reconnected -= OnReconnectedAsync;
        _connection.Closed -= OnClosedAsync;
        await _connection.DisposeAsync().ConfigureAwait(false);
        lock (_stateLock)
        {
            _ready.TrySetResult(false);
            SignalConnectionChanged();
        }
    }

    private async Task InvokeAsync(Func<CancellationToken, Task> invoke, CancellationToken cancellationToken) =>
        await InvokeAsync(
                async token =>
                {
                    await invoke(token).ConfigureAwait(false);
                    return true;
                },
                cancellationToken)
            .ConfigureAwait(false);

    internal async Task<T> InvokeAsync<T>(Func<CancellationToken, Task<T>> invoke, CancellationToken cancellationToken)
    {
        while (true)
        {
            var generation = await WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await invoke(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception) when (exception is not HubException && !cancellationToken.IsCancellationRequested)
            {
                if (!await ConnectionChangesAsync(generation, exception, cancellationToken).ConfigureAwait(false))
                {
                    throw;
                }

                // The connection dropped mid-invocation; wait for re-registration and retry.
                _logger.LogDebug(exception, "Master invocation interrupted by a reconnect; retrying");
            }
        }
    }

    private async Task<long> WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        Task<bool> ready;
        long generation;
        lock (_stateLock)
        {
            ready = _ready.Task;
            generation = _generation;
        }

        if (!await ready.WaitAsync(cancellationToken).ConfigureAwait(false))
        {
            throw new MasterConnectionClosedException();
        }

        return generation;
    }

    /// <summary>
    /// Decides whether a failed invocation was caused by the connection changing.
    /// </summary>
    /// <remarks>
    /// The transport can fail a pending invocation before <see cref="HubConnection"/> raises
    /// <see cref="HubConnection.Closed"/> or <see cref="HubConnection.Reconnecting"/>, so a transport
    /// failure seen while the connection still looks unchanged waits briefly for one of those callbacks.
    /// Any other failure cannot be explained by a connection change that has not happened yet, so it
    /// surfaces immediately.
    /// </remarks>
    private async Task<bool> ConnectionChangesAsync(long generation, Exception exception, CancellationToken cancellationToken)
    {
        lock (_stateLock)
        {
            if (!_ready.Task.IsCompleted)
            {
                return true;
            }
        }

        return await ConnectionChangesSinceAsync(generation, exception, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Decides whether a failure was caused by the connection changing after <paramref name="generation"/>,
    /// waiting briefly for the callback that follows a transport failure.
    /// </summary>
    private async Task<bool> ConnectionChangesSinceAsync(long generation, Exception exception, CancellationToken cancellationToken)
    {
        Task changed;
        lock (_stateLock)
        {
            if (generation != _generation || _connection.State != HubConnectionState.Connected)
            {
                return true;
            }

            if (!IsTransportFailure(exception))
            {
                return false;
            }

            changed = _changed.Task;
        }

        try
        {
            await changed.WaitAsync(ConnectionChangeGracePeriod, cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether an invocation failure can come from the underlying connection failing.
    /// </summary>
    /// <remarks>
    /// <see cref="HubConnection"/> fails pending invocations with the transport error that closed the
    /// connection, or cancels them when it closed without one.
    /// </remarks>
    private static bool IsTransportFailure(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is OperationCanceledException
                or IOException
                or WebSocketException
                or SocketException
                or HttpRequestException
                or ObjectDisposedException
                or TimeoutException)
            {
                return true;
            }
        }

        return false;
    }

    private void SignalConnectionChanged()
    {
        _generation++;
        _changed.TrySetResult();
        _changed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    internal Task OnReconnectingAsync(Exception? exception)
    {
        lock (_stateLock)
        {
            SignalConnectionChanged();
            if (_ready.Task.IsCompleted)
            {
                _ready = CreateReadySignal(completed: false);
            }
        }

        _logger.LogWarning(exception, "Lost the connection to the master; reconnecting");
        return Task.CompletedTask;
    }

    internal async Task OnReconnectedAsync(string? connectionId)
    {
        long generation;
        lock (_stateLock)
        {
            generation = _generation;
        }

        var registration = Volatile.Read(ref _registration);
        var registered = true;
        if (registration is not null)
        {
            try
            {
                // Same registration, same session: the master treats it as a reconnect.
                await RegisterAgainAsync(registration).ConfigureAwait(false);
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Reconnected to the master; worker {WorkerId} registered again", registration.WorkerId);
                }
            }
            catch (Exception exception)
            {
                if (exception is not HubException
                    && await ConnectionChangesSinceAsync(generation, exception, CancellationToken.None).ConfigureAwait(false))
                {
                    // The connection dropped again while registering. The next Reconnected or Closed
                    // callback settles readiness; stopping here would abandon the remaining reconnect attempts.
                    _logger.LogWarning(
                        exception,
                        "Connection to the master dropped again while worker {WorkerId} registered again",
                        registration.WorkerId);
                    return;
                }

                registered = false;
                _logger.LogError(exception, "Worker {WorkerId} could not register again after reconnecting", registration.WorkerId);
            }
        }

        lock (_stateLock)
        {
            SignalConnectionChanged();
            _ready.TrySetResult(registered);
        }

        if (!registered)
        {
            await _connection.StopAsync().ConfigureAwait(false);
        }
    }

    private Task OnClosedAsync(Exception? exception)
    {
        lock (_stateLock)
        {
            SignalConnectionChanged();
            if (_ready.Task.IsCompleted)
            {
                _ready = CreateReadySignal(completed: false);
            }

            _ready.TrySetResult(false);
        }

        return Task.CompletedTask;
    }

    private static TaskCompletionSource<bool> CreateReadySignal(bool completed)
    {
        var signal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (completed)
        {
            signal.SetResult(true);
        }

        return signal;
    }

    internal sealed class MasterConnectionClosedException()
        : InvalidOperationException("The connection to the distributed master is closed.");
}
