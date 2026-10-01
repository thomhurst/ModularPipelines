using ModularPipelines.Context;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised immediately before a module starts.
/// </summary>
public interface IModuleStartHandler : IEventHandler
{
    /// <summary>
    /// Called when the module is about to start executing.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <param name="cancellationToken">A token canceled when the module's execution is canceled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// A handler failure fails the module unless <see cref="IEventHandler.ContinueOnError"/> is set.
    /// </remarks>
    Task OnModuleStartAsync(IModuleHookContext context, CancellationToken cancellationToken);
}
