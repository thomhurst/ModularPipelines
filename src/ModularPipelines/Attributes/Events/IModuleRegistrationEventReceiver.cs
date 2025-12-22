namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive registration events.
/// Invoked during module registration in the DI container.
/// Use for dynamic dependency configuration and service registration.
/// </summary>
public interface IModuleRegistrationEventReceiver
{
    /// <summary>
    /// Called when the module is being registered.
    /// </summary>
    /// <param name="context">The registration context providing access to dependencies and services.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnRegistrationAsync(IModuleRegistrationContext context);
}
