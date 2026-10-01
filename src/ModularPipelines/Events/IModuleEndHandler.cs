using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Events;

/// <summary>
/// Handles the event raised after a module completes successfully.
/// </summary>
public interface IModuleEndHandler : IEventHandler
{
    /// <summary>
    /// Called when the module has finished executing.
    /// </summary>
    /// <param name="context">The module hook context.</param>
    /// <param name="result">The module execution result.</param>
    /// <param name="cancellationToken">A token canceled when the pipeline is canceled by the user or host.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// End handlers observe the outcome and cannot change it. A handler failure is logged and
    /// recorded as a pipeline error; the module keeps its status.
    /// </remarks>
    Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result, CancellationToken cancellationToken);
}
