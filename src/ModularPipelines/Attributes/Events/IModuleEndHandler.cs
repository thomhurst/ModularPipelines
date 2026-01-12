using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to handle module end events.
/// Invoked after a module completes (success or failure).
/// </summary>
public interface IModuleEndHandler
{
    /// <summary>
    /// Gets a value indicating whether to continue execution if this handler throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has finished executing.
    /// </summary>
    /// <param name="context">The hook context providing module information and control flow.</param>
    /// <param name="result">The result of the module execution.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result);
}
