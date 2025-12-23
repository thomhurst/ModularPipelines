namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module ready events.
/// Invoked when all dependencies of a module have completed and the module is ready to execute.
/// This event fires before the module enters the execution queue.
/// </summary>
public interface IModuleReadyEventReceiver
{
    /// <summary>
    /// Gets a value indicating whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when all dependencies of the module have completed and it is ready to execute.
    /// </summary>
    /// <param name="context">The event context providing module information and timing data.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleReadyAsync(IModuleReadyContext context);
}
