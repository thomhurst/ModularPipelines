using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Executes the modules selected by the pipeline planner.
/// </summary>
/// <remarks>
/// Implement this interface to provide a custom orchestration backend, such as a cloud task queue
/// or one isolated process per module. Before completing, a backend must either return or apply a
/// result for every planned module.
/// </remarks>
public interface IExecutionBackend
{
    /// <summary>
    /// Executes the planned modules and returns their results.
    /// </summary>
    /// <param name="modules">The planned modules to execute.</param>
    /// <param name="context">Operations supplied by the engine for applying remotely produced results.</param>
    /// <param name="cancellationToken">A token that requests pipeline cancellation.</param>
    /// <returns>The completed module results. Results already applied through <paramref name="context"/> may be omitted.</returns>
    Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IExecutionBackendContext context,
        CancellationToken cancellationToken);
}
