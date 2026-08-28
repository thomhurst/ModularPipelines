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
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleFailureAsync(IModuleHookContext context, Exception exception);
}
