using ModularPipelines.Context;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Hooks for module-level lifecycle events.
/// </summary>
public interface IPipelineModuleHooks
{
    /// <summary>
    /// Called when a module's dependencies are satisfied and it's ready to execute.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnModuleReadyAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module starts executing.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module completes (success or failure).
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnModuleEndAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module fails with an exception.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnModuleFailureAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module is skipped.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnModuleSkippedAsync(IModuleHookContext context) => Task.CompletedTask;
}
