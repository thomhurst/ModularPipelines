using ModularPipelines.Context;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to handle module start events.
/// Invoked immediately before a module starts executing.
/// </summary>
public interface IModuleStartHandler
{
    /// <summary>
    /// Gets a value indicating whether to continue execution if this handler throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module is about to start executing.
    /// </summary>
    /// <param name="context">The hook context providing module information and control flow.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleStartAsync(IModuleHookContext context);
}
