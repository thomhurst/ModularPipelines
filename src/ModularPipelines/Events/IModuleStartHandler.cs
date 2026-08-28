using ModularPipelines.Context;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised immediately before a module starts.
/// </summary>
public interface IModuleStartHandler : IEventHandler
{
    /// <summary>
    /// Called when the module is about to start executing.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleStartAsync(IModuleHookContext context);
}
