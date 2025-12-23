namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module failure events.
/// Invoked when a module fails with an exception.
/// Called before OnModuleEndAsync.
/// </summary>
public interface IModuleFailureEventReceiver
{
    /// <summary>
    /// Gets a value indicating whether gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has failed with an exception.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleFailureAsync(IModuleEventContext context, Exception exception);
}
