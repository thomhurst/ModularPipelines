using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Interface for scheduling modules across distributed nodes.
/// </summary>
public interface IDistributedScheduler
{
    /// <summary>
    /// Creates an execution plan for the given modules across available workers.
    /// </summary>
    /// <param name="modules">The modules to schedule.</param>
    /// <param name="availableNodes">The available execution nodes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An execution plan mapping modules to execution nodes.</returns>
    Task<DistributedExecutionPlan> CreateExecutionPlanAsync(
        IReadOnlyList<ModuleBase> modules,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reschedules a module if its assigned node becomes unavailable.
    /// </summary>
    /// <param name="module">The module to reschedule.</param>
    /// <param name="availableNodes">The currently available execution nodes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The execution node to run the module, or null if none available.</returns>
    Task<IExecutionNode?> RescheduleModuleAsync(
        ModuleBase module,
        IReadOnlyList<IExecutionNode> availableNodes,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents an execution plan for distributed module execution.
/// </summary>
public sealed class DistributedExecutionPlan
{
    /// <summary>
    /// Gets the mapping of modules to their assigned execution nodes.
    /// </summary>
    public required IReadOnlyDictionary<ModuleBase, IExecutionNode> ModuleAssignments { get; init; }

    /// <summary>
    /// Gets the estimated execution time for the plan.
    /// </summary>
    public TimeSpan EstimatedDuration { get; init; }

    /// <summary>
    /// Gets the execution waves (modules that can execute in parallel).
    /// </summary>
    public required IReadOnlyList<IReadOnlyList<ModuleBase>> ExecutionWaves { get; init; }
}
