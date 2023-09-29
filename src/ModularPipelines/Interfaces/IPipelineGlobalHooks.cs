using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Used to hook into before and after all modules have run
/// </summary>
public interface IPipelineGlobalHooks
{
    /// <summary>
    /// A hook to run before any modules have started
    /// </summary>
    /// <param name="pipelineContext">A pipeline context object provided by the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnStartAsync(IPipelineHookContext pipelineContext);

    /// <summary>
    /// A hook to run after all modules have finished
    /// </summary>
    /// <param name="pipelineContext">A pipeline context object provided by the pipeline.</param>
    /// <param name="pipelineSummary">A summary of the pipeline results, containing all of the registered modules.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnEndAsync(IPipelineHookContext pipelineContext, PipelineSummary pipelineSummary);
}
