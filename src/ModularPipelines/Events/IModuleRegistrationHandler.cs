namespace ModularPipelines.Events;

/// <summary>
/// Handles module registration for an attribute.
/// </summary>
public interface IModuleRegistrationHandler : IEventHandler
{
    /// <summary>
    /// Called when the module is being registered.
    /// </summary>
    /// <param name="context">The module registration context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnRegistrationAsync(IModuleRegistrationContext context);
}
