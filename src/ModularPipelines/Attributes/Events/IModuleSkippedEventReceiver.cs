using ModularPipelines.Models;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module skipped events.
/// Invoked when a module is skipped.
/// </summary>
public interface IModuleSkippedEventReceiver
{
    /// <summary>
    /// Gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has been skipped.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <param name="reason">The reason the module was skipped.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleSkippedAsync(IModuleEventContext context, SkipDecision reason);
}
