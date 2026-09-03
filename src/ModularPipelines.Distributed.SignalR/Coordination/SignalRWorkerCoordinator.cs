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

    private WorkerRegistration? _lastRegistration;
    private ModuleAssignment? _inFlightAssignment;
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
        _connection.On<SerializedModuleResult>(HubMethodNames.ReceiveDependencyResult, OnReceiveDependencyResult);
        _connection.On(HubMethodNames.SignalCompletion, OnSignalCompletion);
        _connection.On(
            HubMethodNames.BroadcastCancellation,
            () => _cancellationRequested.TrySetResult());

        // Automatic reconnect gives us a new connection id, and the master drops the old
        // connection (re-queuing its in-flight work). Re-register under the new connection
        // and ask for work again so the master resumes dispatching to us; otherwise a
        // reconnected worker is orphaned and sits idle.
        _connection.Reconnected += OnReconnectedAsync;
    }

    /// <summary>
    /// Event raised when a dependency result is received from the master.
    /// The worker executor can subscribe to pre-populate CompletionSources.
    /// </summary>
    public event Action<SerializedModuleResult>? DependencyResultReceived;

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
            await _connection.InvokeAsync(HubMethodNames.RequestWork, workerCapabilities, cancellationToken);

            // Wait for assignment via the channel (populated by ReceiveAssignment callback)
            if (await _assignmentChannel.Reader.WaitToReadAsync(cancellationToken))
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
        await _connection.InvokeAsync(HubMethodNames.PublishResult, result, cancellationToken);

        var assignment = Volatile.Read(ref _inFlightAssignment);
        if (assignment?.ModuleTypeName == result.ModuleTypeName)
        {
            Interlocked.CompareExchange(ref _inFlightAssignment, null, assignment);
        }
    }

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        _lastRegistration = registration;
        await _connection.InvokeAsync(
            HubMethodNames.RegisterWorker,
            registration,
            Volatile.Read(ref _inFlightAssignment)?.ModuleTypeName,
            cancellationToken);
        _logger.LogInformation("Worker {Index} registered with master via SignalR", registration.WorkerIndex);
    }

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        _connection.InvokeAsync(HubMethodNames.Heartbeat, status, cancellationToken);

    public Task WaitForCancellationAsync(CancellationToken cancellationToken) =>
        _cancellationRequested.Task.WaitAsync(cancellationToken);

    private async Task OnReconnectedAsync(string? connectionId)
    {
        var registration = _lastRegistration;
        if (registration is null)
        {
            return;
        }

        try
        {
            _logger.LogWarning(
                "Reconnected to master (connection {ConnectionId}); re-registering worker {Index}",
                connectionId, registration.WorkerIndex);

            await _connection.InvokeAsync(
                HubMethodNames.RegisterWorker,
                registration,
                Volatile.Read(ref _inFlightAssignment)?.ModuleTypeName);

            // Only re-request work if we're idle and waiting for an assignment. If we're
            // mid-execution, the master restored our in-flight module on re-registration;
            // requesting work now would make it dispatch that module again (double run).
            if (_awaitingAssignment)
            {
                await _connection.InvokeAsync(HubMethodNames.RequestWork, registration.Capabilities);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to re-register worker {Index} after reconnect", registration.WorkerIndex);
        }
    }

    private void OnReceiveAssignment(ModuleAssignment assignment)
    {
        _logger.LogDebug("Received assignment: {Module}", assignment.ModuleTypeName);
        Volatile.Write(ref _inFlightAssignment, assignment);
        _assignmentChannel.Writer.TryWrite(assignment);
    }

    private void OnReceiveDependencyResult(SerializedModuleResult result)
    {
        _logger.LogDebug("Received dependency result: {Module}", result.ModuleTypeName);
        DependencyResultReceived?.Invoke(result);
    }

    private void OnSignalCompletion()
    {
        _logger.LogInformation("Received completion signal from master");
        _assignmentChannel.Writer.TryComplete();
    }
}
