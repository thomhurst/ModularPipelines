using ModularPipelines.Models;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module end events.
/// Invoked after a module completes (success or failure).
/// </summary>
public interface IModuleEndEventReceiver
{
    /// <summary>
    /// Gets a value indicating whether gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has finished executing.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <param name="result">The result of the module execution.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleEndAsync(IModuleEventContext context, ModuleResult result);
}
