namespace ModularPipelines.Distributed;

/// <summary>
/// Represents a worker's claim on a <see cref="ModuleAssignment"/>.
/// </summary>
/// <remarks>
/// A lease stays valid while heartbeats from its worker list the module in
/// <see cref="WorkerStatus.InFlightModules"/>. When the lease expires before a result is published,
/// <see cref="IDistributedMasterCoordinator.RequeueExpiredLeasesAsync"/> returns the assignment to the queue.
/// </remarks>
public sealed record ModuleLease
{
    /// <summary>Gets the unique identifier of this claim.</summary>
    public required string LeaseId { get; init; }

    /// <summary>Gets the worker that holds the lease.</summary>
    public required WorkerId WorkerId { get; init; }

    /// <summary>Gets the claimed assignment.</summary>
    public required ModuleAssignment Assignment { get; init; }
}
