using ModularPipelines.Context;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised when a module fails.
/// </summary>
public interface IModuleFailureHandler : IEventHandler
{
    /// <summary>
    /// Called when the module fails with an exception.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    /// <param name="cancellationToken">A token cancelled when the pipeline is cancelled by the user or host.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Failure handlers observe the outcome and cannot change it. A handler failure is logged and
    /// recorded as a pipeline error; it never replaces the module's exception.
    /// </remarks>
    Task OnModuleFailureAsync(IModuleHookContext context, Exception exception, CancellationToken cancellationToken);
}
