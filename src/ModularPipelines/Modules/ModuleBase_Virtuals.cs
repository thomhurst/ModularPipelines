using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public partial class ModuleBase
{
    /// <summary>
    /// A Timeout for the module
    /// </summary>
    protected virtual TimeSpan Timeout => TimeSpan.FromMinutes(30);

    /// <summary>
    /// If true, the pipeline will not fail is this module fails.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    [ModuleMethodMarker]
    protected virtual Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception) => Task.FromResult(false);

    /// <summary>
    /// If true, this module will not run.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [ModuleMethodMarker]
    protected virtual Task<bool> ShouldSkip(IPipelineContext context) => Task.FromResult(false);

    /// <summary>
    /// If this module is skipped, and this returns true, the result of this module will be reconstructed from the plugged in <see cref="IModuleResultRepository"/>.
    /// If no persisted result can be reconstructed, this module will fail.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [ModuleMethodMarker]
    protected virtual Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context) => Task.FromResult(context.ModuleResultRepository.GetType() != typeof(NoOpModuleResultRepository));

    public virtual ModuleRunType ModuleRunType => ModuleRunType.OnSuccessfulDependencies;
    
    [ModuleMethodMarker]
    protected virtual Task OnBeforeExecute(IPipelineContext context)
    {
        return Task.CompletedTask;
    }

    [ModuleMethodMarker]
    protected virtual Task OnAfterExecute(IPipelineContext context)
    {
        return Task.CompletedTask;
    }
}