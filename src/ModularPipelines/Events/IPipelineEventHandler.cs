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
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnPipelineStartAsync(IPipelineContext context) => Task.CompletedTask;

    /// <summary>
    /// Called after all modules finish.
    /// </summary>
    /// <param name="context">The pipeline hook context.</param>
    /// <param name="pipelineSummary">The summary of all registered module results.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnPipelineEndAsync(IPipelineContext context, PipelineSummary pipelineSummary) => Task.CompletedTask;
}
