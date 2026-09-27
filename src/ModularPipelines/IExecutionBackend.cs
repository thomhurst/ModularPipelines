using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Executes the modules selected by the pipeline planner.
/// </summary>
/// <remarks>
/// Implement this interface to provide a custom orchestration backend that supplies module results,
/// for example by submitting modules to a cloud task queue or to remote processes and returning the
/// results they produce, or by calling <see cref="IExecutionBackendContext.ExecuteModuleAsync"/>
/// to execute planned modules through the engine's in-process lifecycle.
/// A backend that <see cref="OwnsEntirePlan"/> must either return or apply a result for every planned
/// module before completing.
/// </remarks>
public interface IExecutionBackend
{
    /// <summary>
    /// Gets a value indicating whether this backend owns every module in the supplied execution plan.
    /// </summary>
    /// <remarks>
    /// Return <see langword="false"/> when this process executes only a claimed subset of the plan.
    /// Such a backend must return a result for every module it claimed, whether it succeeded or
    /// failed; those results form this process's pipeline summary.
    /// </remarks>
    bool OwnsEntirePlan { get; }

    /// <summary>
    /// Executes the planned modules and returns their results.
    /// </summary>
    /// <param name="modules">The planned modules to execute.</param>
    /// <param name="estimatedDurations">
    /// Historical duration estimates keyed by module type, used to prioritise scheduling. Modules
    /// without history are absent from the dictionary.
    /// </param>
    /// <param name="context">Operations for executing planned modules locally and applying remotely produced results.</param>
    /// <param name="cancellationToken">A token that requests pipeline cancellation.</param>
    /// <returns>
    /// The completed module results. Each returned result must provide its module's fully qualified
    /// type name through <see cref="IModuleResult.TypeName"/>. A backend that
    /// <see cref="OwnsEntirePlan"/> may omit results already applied through
    /// <paramref name="context"/>; any other backend must return the result of every module it claimed.
    /// </returns>
    Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
        IExecutionBackendContext context,
        CancellationToken cancellationToken);
}
