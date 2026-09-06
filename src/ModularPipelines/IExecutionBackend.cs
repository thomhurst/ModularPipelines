using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Executes the modules selected by the pipeline planner.
/// </summary>
/// <remarks>
/// Implement this interface to provide a custom orchestration backend, such as a cloud task queue
/// or one isolated process per module. A backend that <see cref="OwnsEntirePlan"/> must either return
/// or apply a result for every planned module before completing.
/// </remarks>
public interface IExecutionBackend
{
    /// <summary>
    /// Gets a value indicating whether this backend owns every module in the supplied execution plan.
    /// </summary>
    /// <remarks>
    /// Return <see langword="false"/> when this process executes only a claimed subset of the plan.
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
    /// <param name="context">Operations supplied by the engine for applying remotely produced results.</param>
    /// <param name="cancellationToken">A token that requests pipeline cancellation.</param>
    /// <returns>
    /// The completed module results. Each returned result must provide its module's fully qualified
    /// type name through <see cref="IModuleResult.TypeName"/>. Results already applied through
    /// <paramref name="context"/> may be omitted.
    /// </returns>
    Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
        IExecutionBackendContext context,
        CancellationToken cancellationToken);
}
