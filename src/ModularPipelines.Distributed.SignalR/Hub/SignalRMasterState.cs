using System.Collections.Concurrent;
using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// State shared by the SignalR master coordinator and its hub. The in-memory coordinator is the
/// single source of truth for the queue, leases, results and cancellation; the hub only
/// authenticates connections and forwards worker calls to it.
/// </summary>
internal sealed class SignalRMasterState(InMemoryDistributedCoordinator coordinator, string runId)
{
    private bool _completed;

    /// <summary>Gets the authoritative coordinator.</summary>
    public InMemoryDistributedCoordinator Coordinator { get; } = coordinator;

    /// <summary>Gets the run every worker must belong to.</summary>
    public string RunId { get; } = runId;

    /// <summary>Gets the registered worker session for each connection.</summary>
    public ConcurrentDictionary<string, WorkerRegistration> Sessions { get; } = new(StringComparer.Ordinal);

    /// <summary>Gets the connections that acknowledged the master's completion.</summary>
    public ConcurrentDictionary<string, bool> CompletionAcknowledgements { get; } = new(StringComparer.Ordinal);

    /// <summary>Gets a value indicating whether the master has told its workers it completed.</summary>
    public bool IsCompleted => Volatile.Read(ref _completed);

    /// <summary>Records that the master completed.</summary>
    public void MarkCompleted() => Volatile.Write(ref _completed, true);

    /// <summary>
    /// Determines whether every registered connection has acknowledged the master's completion.
    /// </summary>
    public bool AllWorkersAcknowledgedCompletion() =>
        Sessions.Keys.All(CompletionAcknowledgements.ContainsKey);
}
