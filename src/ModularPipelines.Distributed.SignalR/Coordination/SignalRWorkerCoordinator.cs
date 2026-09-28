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
    private WorkerRegistration? _registration;
    private long _generation;

    public SignalRWorkerCoordinator(HubConnection connection, ILogger<SignalRWorkerCoordinator> logger)
    {
        _connection = connection;
        _logger = logger;
        _connection.Reconnecting += OnReconnectingAsync;
        _connection.Reconnected += OnReconnectedAsync;
        _connection.Closed += OnClosedAsync;
    }

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

    private async Task<T> InvokeAsync<T>(Func<CancellationToken, Task<T>> invoke, CancellationToken cancellationToken)
    {
        while (true)
        {
            var generation = await WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await invoke(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception) when (
                exception is not HubException
                && !cancellationToken.IsCancellationRequested
                && ConnectionChangedSince(generation))
            {
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

    private bool ConnectionChangedSince(long generation)
    {
        lock (_stateLock)
        {
            return generation != _generation
                   || !_ready.Task.IsCompleted
                   || _connection.State != HubConnectionState.Connected;
        }
    }

    private Task OnReconnectingAsync(Exception? exception)
    {
        lock (_stateLock)
        {
            _generation++;
            if (_ready.Task.IsCompleted)
            {
                _ready = CreateReadySignal(completed: false);
            }
        }

        _logger.LogWarning(exception, "Lost the connection to the master; reconnecting");
        return Task.CompletedTask;
    }

    private async Task OnReconnectedAsync(string? connectionId)
    {
        var registration = Volatile.Read(ref _registration);
        var registered = true;
        if (registration is not null)
        {
            try
            {
                // Same registration, same session: the master treats it as a reconnect.
                await _connection.InvokeAsync(HubMethodNames.RegisterWorker, registration).ConfigureAwait(false);
                _logger.LogInformation("Reconnected to the master; worker {WorkerId} registered again", registration.WorkerId);
            }
            catch (Exception exception)
            {
                registered = false;
                _logger.LogError(exception, "Worker {WorkerId} could not register again after reconnecting", registration.WorkerId);
            }
        }

        lock (_stateLock)
        {
            _generation++;
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
            _generation++;
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

    private sealed class MasterConnectionClosedException()
        : InvalidOperationException("The connection to the distributed master is closed.");
}
