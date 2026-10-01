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
    /// <param name="cancellationToken">A token canceled when the pipeline is canceled by the user or host.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Skipped handlers observe the outcome and cannot change it. A handler failure is logged and
    /// recorded as a pipeline error; the module stays skipped.
    /// </remarks>
    Task OnModuleSkippedAsync(IModuleHookContext context, SkipDecision reason, CancellationToken cancellationToken);
}
