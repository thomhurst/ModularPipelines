using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Events;

/// <summary>
/// Handles pipeline-level lifecycle events.
/// </summary>
public interface IPipelineEventHandler : IEventHandler
{
    /// <summary>
    /// Called before any modules start.
    /// </summary>
    /// <param name="context">The pipeline hook context.</param>
    /// <param name="cancellationToken">A token canceled when the pipeline is canceled.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// Called after all modules finish.
    /// </summary>
    /// <param name="context">The pipeline hook context.</param>
    /// <param name="pipelineSummary">The summary of all registered module results.</param>
    /// <param name="cancellationToken">
    /// A token canceled when the pipeline is canceled by the user or host. Module failures do not cancel it.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// A handler failure fails the pipeline, but never replaces an exception that already failed the
    /// pipeline: in that case it is logged and recorded as an additional pipeline error.
    /// </remarks>
    Task OnPipelineEndAsync(
        IPipelineContext context,
        PipelineSummary pipelineSummary,
        CancellationToken cancellationToken) => Task.CompletedTask;
}
