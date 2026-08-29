using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised when a module is skipped.
/// </summary>
public interface IModuleSkippedHandler : IEventHandler
{
    /// <summary>
    /// Called when the module is skipped.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <param name="reason">The decision that caused the module to be skipped.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleSkippedAsync(IModuleHookContext context, SkipDecision reason);
}
