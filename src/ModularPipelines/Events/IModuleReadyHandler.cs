using ModularPipelines.Context;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised when a module's dependencies are satisfied.
/// </summary>
public interface IModuleReadyHandler : IEventHandler
{
    /// <summary>
    /// Called when the module is ready to execute.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleReadyAsync(IModuleHookContext context);
}
