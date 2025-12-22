namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module start events.
/// Invoked immediately before a module starts executing.
/// </summary>
public interface IModuleStartEventReceiver
{
    /// <summary>
    /// Gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module is about to start executing.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleStartAsync(IModuleEventContext context);
}
