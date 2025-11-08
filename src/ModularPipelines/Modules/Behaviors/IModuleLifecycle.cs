using ModularPipelines.Context;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to provide lifecycle hooks for a module.
/// Hooks allow you to run code before and after module execution.
/// </summary>
public interface IModuleLifecycle
{
    /// <summary>
    /// Called before the module's ExecuteAsync method is invoked.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnBeforeExecuteAsync(IPipelineContext context);

    /// <summary>
    /// Called after the module's ExecuteAsync method completes (success or failure).
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <param name="result">The result returned by ExecuteAsync, or null if it failed.</param>
    /// <param name="exception">The exception thrown by ExecuteAsync, or null if it succeeded.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnAfterExecuteAsync(IPipelineContext context, object? result, Exception? exception);
}
