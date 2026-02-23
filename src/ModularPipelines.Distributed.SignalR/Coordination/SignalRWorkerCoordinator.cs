using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Worker-side <see cref="IDistributedCoordinator"/> backed by a SignalR <see cref="HubConnection"/> to the master.
/// Receives work assignments via <c>ReceiveAssignment</c> callback and publishes results via hub invocations.
/// </summary>
internal class SignalRWorkerCoordinator : IDistributedCoordinator
{
    private readonly HubConnection _connection;
    private readonly ILogger<SignalRWorkerCoordinator> _logger;
    private readonly Channel<ModuleAssignment> _assignmentChannel;
    private volatile bool _completed;

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
    }

    /// <summary>
    /// Event raised when a dependency result is received from the master.
    /// The worker executor can subscribe to pre-populate CompletionSources.
    /// </summary>
    public event Action<SerializedModuleResult>? DependencyResultReceived;

    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        // Workers don't enqueue — only the master does.
        throw new NotSupportedException("Workers do not enqueue work. The master pushes assignments via hub callbacks.");
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
    {
        try
        {
            // Request work from master
            await _connection.InvokeAsync(HubMethodNames.RequestWork, workerCapabilities, cancellationToken);

            // Wait for assignment via the channel (populated by ReceiveAssignment callback)
            if (await _assignmentChannel.Reader.WaitToReadAsync(cancellationToken))
            {
                if (_assignmentChannel.Reader.TryRead(out var assignment))
                {
                    return assignment;
                }
            }

            return null; // Channel completed = no more work
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public async Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
    {
        await _connection.InvokeAsync(HubMethodNames.PublishResult, result, cancellationToken);
    }

    public Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        // Workers don't wait for results — only the master does.
        throw new NotSupportedException("Workers do not wait for results. The master waits via its coordinator.");
    }

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        await _connection.InvokeAsync(HubMethodNames.RegisterWorker, registration, cancellationToken);
        _logger.LogInformation("Worker {Index} registered with master via SignalR", registration.WorkerIndex);
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        // Workers don't query registrations — only the master does.
        throw new NotSupportedException("Workers do not query registered workers.");
    }

    public Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        // Workers don't signal completion — only the master does.
        // Workers receive the signal via OnSignalCompletion callback.
        return Task.CompletedTask;
    }

    private void OnReceiveAssignment(ModuleAssignment assignment)
    {
        _logger.LogDebug("Received assignment: {Module}", assignment.ModuleTypeName);
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
        _completed = true;
        _assignmentChannel.Writer.TryComplete();
    }
}
