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
    /// <summary>Gets the authoritative coordinator.</summary>
    public InMemoryDistributedCoordinator Coordinator { get; } = coordinator;

    /// <summary>Gets the run every worker must belong to.</summary>
    public string RunId { get; } = runId;

    /// <summary>Gets the registered worker session for each connection.</summary>
    public ConcurrentDictionary<string, WorkerRegistration> Sessions { get; } = new(StringComparer.Ordinal);
}
