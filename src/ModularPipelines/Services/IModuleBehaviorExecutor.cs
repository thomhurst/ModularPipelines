using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Service responsible for orchestrating module execution with all configured behaviors.
/// This handles skip logic, lifecycle hooks, retry, timeout, error handling, and state tracking.
/// </summary>
public interface IModuleBehaviorExecutor
{
    /// <summary>
    /// Executes a module with all configured behaviors.
    /// </summary>
    /// <typeparam name="T">The type of result the module produces.</typeparam>
    /// <param name="module">The module to execute.</param>
    /// <param name="context">The pipeline context.</param>
    /// <param name="cancellationToken">The cancellation token for this module's execution.</param>
    /// <param name="pipelineCancellationToken">The pipeline's cancellation token to detect pipeline termination.</param>
    /// <returns>The module's result, or null if execution was skipped or failed.</returns>
    Task<T?> ExecuteAsync<T>(IModule<T> module, IPipelineContext context, CancellationToken cancellationToken, CancellationToken pipelineCancellationToken);
}
