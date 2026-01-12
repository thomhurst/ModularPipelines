using ModularPipelines.Context;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to handle module failure events.
/// Invoked when a module fails with an exception.
/// Called before OnModuleEndAsync.
/// </summary>
public interface IModuleFailureHandler
{
    /// <summary>
    /// Gets a value indicating whether to continue execution if this handler throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has failed with an exception.
    /// </summary>
    /// <param name="context">The hook context providing module information and control flow.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleFailureAsync(IModuleHookContext context, Exception exception);
}
