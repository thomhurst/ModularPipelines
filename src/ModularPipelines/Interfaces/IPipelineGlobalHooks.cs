using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Hooks for pipeline-level lifecycle events (before any modules start, after all complete).
/// </summary>
public interface IPipelineGlobalHooks
{
    /// <summary>
    /// Called before any modules have started.
    /// </summary>
    /// <param name="context">A pipeline hook context object provided by the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnPipelineStartAsync(IPipelineHookContext context) => Task.CompletedTask;

    /// <summary>
    /// Called after all modules have finished.
    /// </summary>
    /// <param name="context">A pipeline hook context object provided by the pipeline.</param>
    /// <param name="pipelineSummary">A summary of the pipeline results, containing all of the registered modules.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnPipelineEndAsync(IPipelineHookContext context, PipelineSummary pipelineSummary) => Task.CompletedTask;
}
