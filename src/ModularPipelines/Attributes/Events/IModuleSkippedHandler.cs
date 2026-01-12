using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to handle module skipped events.
/// Invoked when a module is skipped.
/// </summary>
public interface IModuleSkippedHandler
{
    /// <summary>
    /// Gets a value indicating whether to continue execution if this handler throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has been skipped.
    /// </summary>
    /// <param name="context">The hook context providing module information and control flow.</param>
    /// <param name="reason">The reason the module was skipped.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleSkippedAsync(IModuleHookContext context, SkipDecision reason);
}
