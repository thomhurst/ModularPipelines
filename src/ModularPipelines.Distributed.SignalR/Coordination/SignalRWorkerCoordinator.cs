using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Worker-side <see cref="IDistributedWorkerCoordinator"/> backed by a SignalR <see cref="HubConnection"/> to the master.
/// Receives work assignments via <c>ReceiveAssignment</c> callback and publishes results via hub invocations.
/// </summary>
internal class SignalRWorkerCoordinator : IDistributedWorkerCoordinator
{
    private readonly HubConnection _connection;
    private readonly ILogger<SignalRWorkerCoordinator> _logger;
    private readonly Channel<ModuleAssignment> _assignmentChannel;
    private readonly TaskCompletionSource _cancellationRequested = new(
        TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly Lock _reconnectLock = new();

    private WorkerRegistration? _lastRegistration;
    private ModuleAssignment? _inFlightAssignment;
    private TaskCompletionSource<bool> _connectionTransition = new(
        TaskCreationOptions.RunContinuationsAsynchronously);
    private long _connectionGeneration;
    private bool _reconnecting;
    private bool _lastReconnectSucceeded;
    private volatile bool _awaitingAssignment;

    public SignalRWorkerCoordinator(HubConnection connection, ILogger<SignalRWorkerCoordinator> logger)
    {
        _connection = connection;
        _logger = logger;
        _assignmentChannel = Channel.CreateUnbounded<ModuleAssignment>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
        });

        // Register callbacks for master -> worker methods
        _connection.On<ModuleAssignment>(HubMethodNames.ReceiveAssignment, OnReceiveAssignment);
        _connection.On(HubMethodNames.SignalCompletion, OnSignalCompletion);
        _connection.On(
            HubMethodNames.BroadcastCancellation,
            () => _cancellationRequested.TrySetResult());

        // Automatic reconnect gives us a new connection id, and the master drops the old
        // connection (re-queuing its in-flight work). Re-register under the new connection
        // and ask for work again so the master resumes dispatching to us; otherwise a
        // reconnected worker is orphaned and sits idle.
        _connection.Reconnecting += OnReconnectingAsync;
        _connection.Reconnected += OnReconnectedAsync;
        _connection.Closed += OnClosedAsync;
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<Capability> workerCapabilities, CancellationToken cancellationToken)
    {
        try
        {
            // Mark that we're waiting for an assignment. Used by the reconnect handler to
            // decide whether to re-request work: if we're mid-execution (not awaiting), a
            // reconnect must NOT request new work or the master would dispatch the module
            // it already re-tracked for us, running it twice.
            _awaitingAssignment = true;

            // Request work from master
            await _connection.InvokeAsync(HubMethodNames.RequestWork, workerCapabilities, cancellationToken)
                .ConfigureAwait(false);

            // Wait for assignment via the channel (populated by ReceiveAssignment callback)
            if (await _assignmentChannel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (_assignmentChannel.Reader.TryRead(out var assignment))
                {
                    _awaitingAssignment = false;
                    return assignment;
                }
            }

            return null; // Channel completed = no more work
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        finally
        {
            _awaitingAssignment = false;
        }
    }

    public async Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
    {
        while (true)
        {
            var connectionGeneration = await WaitForRegistrationAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                await _connection.InvokeAsync(HubMethodNames.PublishResult, result, cancellationToken)
                    .ConfigureAwait(false);
                break;
            }
            catch (Exception ex) when (CanRetryHubInvocation(ex, connectionGeneration, cancellationToken))
            {
                if (!await WaitForReconnectAsync(connectionGeneration, cancellationToken).ConfigureAwait(false))
                {
                    throw;
                }
            }
        }

        var assignment = Volatile.Read(ref _inFlightAssignment);
        if (assignment?.ModuleTypeName == result.ModuleTypeName)
        {
            Interlocked.CompareExchange(ref _inFlightAssignment, null, assignment);
        }
    }

    private async Task<long> WaitForRegistrationAsync(CancellationToken cancellationToken)
    {
        long connectionGeneration;
        Task<bool>? registration;
        lock (_reconnectLock)
        {
            connectionGeneration = _connectionGeneration;
            registration = _reconnecting ? _connectionTransition.Task : null;
        }

        if (registration is not null
            && !await registration.WaitAsync(cancellationToken).ConfigureAwait(false))
        {
            throw new InvalidOperationException("Worker registration failed after reconnecting to the master.");
        }

        return connectionGeneration;
    }

    private bool CanRetryHubInvocation(
        Exception exception,
        long connectionGeneration,
        CancellationToken cancellationToken) =>
        !cancellationToken.IsCancellationRequested
        && (exception is not Microsoft.AspNetCore.SignalR.HubException
            || HasReconnectSince(connectionGeneration));

    public async Task<SerializedModuleResult> WaitForResultAsync(
        string moduleTypeName,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            var connectionGeneration = await WaitForRegistrationAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await _connection.InvokeAsync<SerializedModuleResult>(
                        HubMethodNames.WaitForResult,
                        moduleTypeName,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex) when (CanRetryHubInvocation(ex, connectionGeneration, cancellationToken))
            {
                if (!await WaitForReconnectAsync(connectionGeneration, cancellationToken)
                        .ConfigureAwait(false))
                {
                    throw;
                }
            }
        }
    }

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        _lastRegistration = registration;
        await _connection.InvokeAsync(
            HubMethodNames.RegisterWorker,
            registration,
            Volatile.Read(ref _inFlightAssignment)?.ModuleTypeName,
            cancellationToken).ConfigureAwait(false);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker {Index} registered with master via SignalR", registration.WorkerIndex);
        }
    }

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        _connection.InvokeAsync(HubMethodNames.Heartbeat, status, cancellationToken);

    public Task WaitForCancellationAsync(CancellationToken cancellationToken) =>
        _cancellationRequested.Task.WaitAsync(cancellationToken);

    private Task OnReconnectingAsync(Exception? exception)
    {
        lock (_reconnectLock)
        {
            _reconnecting = true;
            if (_connectionTransition.Task.IsCompleted)
            {
                _connectionTransition = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            }
        }

        return Task.CompletedTask;
    }

    private async Task OnReconnectedAsync(string? connectionId)
    {
        TaskCompletionSource<bool> connectionTransition;
        lock (_reconnectLock)
        {
            connectionTransition = _connectionTransition;
        }

        var registration = _lastRegistration;
        var registered = false;
        try
        {
            if (registration is null)
            {
                registered = true;
                return;
            }

            _logger.LogWarning(
                "Reconnected to master (connection {ConnectionId}); re-registering worker {Index}",
                connectionId, registration.WorkerIndex);

            await _connection.InvokeAsync(
                HubMethodNames.RegisterWorker,
                registration,
                Volatile.Read(ref _inFlightAssignment)?.ModuleTypeName).ConfigureAwait(false);

            // Only re-request work if we're idle and waiting for an assignment. If we're
            // mid-execution, the master restored our in-flight module on re-registration;
            // requesting work now would make it dispatch that module again (double run).
            if (_awaitingAssignment)
            {
                await _connection.InvokeAsync(HubMethodNames.RequestWork, registration.Capabilities)
                    .ConfigureAwait(false);
            }

            registered = true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to re-register worker {Index} after reconnect", registration?.WorkerIndex);
        }
        finally
        {
            // Publication retries must wait until the master has restored assignment ownership.
            lock (_reconnectLock)
            {
                Interlocked.Increment(ref _connectionGeneration);
                _lastReconnectSucceeded = registered;
                _reconnecting = false;
                connectionTransition.TrySetResult(registered);
                _connectionTransition = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            }
        }
    }

    private Task OnClosedAsync(Exception? exception)
    {
        lock (_reconnectLock)
        {
            _reconnecting = false;
            _lastReconnectSucceeded = false;
            _connectionTransition.TrySetResult(false);
        }

        return Task.CompletedTask;
    }

    private async Task<bool> WaitForReconnectAsync(
        long connectionGeneration,
        CancellationToken cancellationToken)
    {
        Task<bool> connectionTransitionTask;
        lock (_reconnectLock)
        {
            if (_connectionGeneration != connectionGeneration)
            {
                return _lastReconnectSucceeded;
            }

            connectionTransitionTask = _connectionTransition.Task;
        }

        return await connectionTransitionTask.WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    private bool HasReconnectSince(long connectionGeneration)
    {
        lock (_reconnectLock)
        {
            return _reconnecting || _connectionGeneration != connectionGeneration;
        }
    }

    private void OnReceiveAssignment(ModuleAssignment assignment)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Received assignment: {Module}", assignment.ModuleTypeName);
        }
        Volatile.Write(ref _inFlightAssignment, assignment);
        _assignmentChannel.Writer.TryWrite(assignment);
    }

    private void OnSignalCompletion()
    {
        _logger.LogInformation("Received completion signal from master");
        _assignmentChannel.Writer.TryComplete();
    }
}
