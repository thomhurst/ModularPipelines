using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised after a module completes successfully.
/// </summary>
public interface IModuleEndHandler : IEventHandler
{
    /// <summary>
    /// Called when the module has finished executing.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <param name="result">The module execution result.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result);
}
