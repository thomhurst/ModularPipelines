using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Resolves the final status of a module based on execution results and pipeline state.
/// </summary>
public interface IModuleStateResolver
{
    /// <summary>
    /// Determines the appropriate final status for a module based on the exception that occurred
    /// and whether the pipeline was cancelled.
    /// </summary>
    /// <param name="module">The module whose status needs to be determined.</param>
    /// <param name="exception">The exception that occurred during module execution, if any.</param>
    /// <param name="pipelineCancellationToken">The engine's cancellation token to check if pipeline was cancelled.</param>
    /// <returns>The appropriate status for the module.</returns>
    Status ResolveFailureStatus(IModule module, Exception exception, CancellationToken pipelineCancellationToken);
}
