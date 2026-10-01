using ModularPipelines.Context;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised when a module's dependencies are satisfied.
/// </summary>
public interface IModuleReadyHandler : IEventHandler
{
    /// <summary>
    /// Called when the module is ready to execute.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <param name="cancellationToken">A token canceled when the module's execution is canceled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// A handler failure fails the module unless <see cref="IEventHandler.ContinueOnError"/> is set.
    /// </remarks>
    Task OnModuleReadyAsync(IModuleHookContext context, CancellationToken cancellationToken);
}
