using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Distributed.Coordination;

// Redis stores this terminal marker atomically with lease removal, then converts it to the
// normal result wire format on read. No module type or worker process is needed to publish failure.
internal sealed record LeaseExpirationFailure(
    ModuleId ModuleId,
    IReadOnlyList<WorkerId> LeaseExpirationWorkers,
    DateTimeOffset CompletedAt)
{
    public SerializedModuleResult ToResult() =>
        ModuleResultSerializer.CreateFailure(
            ModuleId,
            new InvalidOperationException(
                $"Module '{ModuleId}' failed after {LeaseExpirationWorkers.Count} lease expirations. "
                + $"Lost workers: {string.Join(", ", LeaseExpirationWorkers)}."),
            LeaseExpirationWorkers[^1],
            CompletedAt);
}
