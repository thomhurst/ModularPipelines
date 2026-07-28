using ModularPipelines.Context;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Receives lifecycle events for every module in a pipeline.
/// </summary>
/// <remarks>
/// <para>
/// Register implementations with <c>AddModuleEventReceiver&lt;TReceiver&gt;()</c>.
/// All registered receivers are invoked concurrently for each event.
/// </para>
/// <para>
/// Global ready and start receivers run before attribute event handlers and module virtual hooks.
/// For completion events, module virtual hooks run first, followed by attribute event handlers
/// where applicable and then global failure or skipped receivers. Successful end receivers run
/// before attribute end handlers.
/// </para>
/// </remarks>
public interface IModuleEventReceiver
{
    /// <summary>
    /// Called when a module's dependencies are satisfied and it is ready to execute.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleReadyAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module starts executing.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called after a module completes successfully.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleEndAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module fails with an exception.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleFailureAsync(IModuleHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called when a module is skipped.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleSkippedAsync(IModuleHookContext context) => Task.CompletedTask;
}
