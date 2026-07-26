using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// Shared mutable state for the SignalR master coordinator and hub.
/// Thread-safe via concurrent collections and atomic operations.
/// </summary>
internal class SignalRMasterState
{
    /// <summary>
    /// Connected workers indexed by SignalR connection ID.
    /// </summary>
    public ConcurrentDictionary<string, WorkerState> Workers { get; } = new();

    /// <summary>
    /// Worker registrations indexed by worker index.
    /// </summary>
    public ConcurrentDictionary<int, WorkerRegistration> Registrations { get; } = new();

    /// <summary>
    /// Pending work assignments waiting for an idle worker.
    /// </summary>
    public ConcurrentQueue<ModuleAssignment> PendingAssignments { get; } = new();

    /// <summary>
    /// Result waiters: module type name -> TCS that completes when the result arrives.
    /// </summary>
    public ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>> ResultWaiters { get; } = new();

    /// <summary>
    /// Workers that disconnected with an in-flight assignment, keyed by worker index
    /// (stable across reconnects). The assignment is re-enqueued only after
    /// <see cref="ReconnectGracePeriod"/> elapses; if the worker reconnects first, the
    /// pending re-enqueue is cancelled so the module isn't run twice on a transient blip.
    /// </summary>
    public ConcurrentDictionary<int, (ModuleAssignment Assignment, CancellationTokenSource Cts)> PendingReconnects { get; } = new();

    /// <summary>
    /// How long to wait for a disconnected worker to reconnect before re-enqueuing its
    /// in-flight work. Should exceed the client's total auto-reconnect window.
    /// </summary>
    public TimeSpan ReconnectGracePeriod { get; set; } = TimeSpan.FromSeconds(45);

    /// <summary>
    /// Volatile completion flag.
    /// </summary>
    public volatile bool IsCompleted;

    /// <summary>
    /// Signals when work is added to <see cref="PendingAssignments"/> or when completion is signalled.
    /// Used by <see cref="Coordination.SignalRMasterCoordinator.DequeueModuleAsync"/> to avoid polling.
    /// </summary>
    public SemaphoreSlim WorkAvailable { get; } = new(0);
}
