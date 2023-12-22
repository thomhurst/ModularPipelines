using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// A base class for all modules.
/// </summary>
public partial class ModuleBase
{
    /// <summary>
    /// Gets a Timeout for the module.
    /// </summary>
    protected internal virtual TimeSpan Timeout => TimeSpan.FromMinutes(30);

    /// <summary>
    /// If true, the pipeline will not fail is this module fails.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    /// <returns>A boolean that if true, will stop this Module from failing the pipeline if it fails.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception) => Task.FromResult(false);

    /// <summary>
    /// Controls whether to skip this module.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A Skip Decision that controls whether to skip a module, along with a reason.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task<SkipDecision> ShouldSkip(IPipelineContext context) => Task.FromResult(SkipDecision.DoNotSkip);

    /// <summary>
    /// If this module is skipped, and this returns true, the result of this module will be reconstructed from the plugged in <see cref="IModuleResultRepository"/>.
    /// If no persisted result can be reconstructed, this module will fail.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A boolean controlling whether to use historical data if available.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context) => Task.FromResult(context.ModuleResultRepository.GetType() != typeof(NoOpModuleResultRepository));

    /// <summary>
    /// A hook that runs before the module is started.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task OnBeforeExecute(IPipelineContext context)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// A hook that runs after the module has finished executing.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task OnAfterExecute(IPipelineContext context)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets whether the Module should run even if the pipeline has failed.
    /// </summary>
    public virtual ModuleRunType ModuleRunType => ModuleRunType.OnSuccessfulDependencies;
}