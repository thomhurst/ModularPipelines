using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Modules;

/// <summary>
/// A base class for all modules.
/// </summary>
public partial class ModuleBase
{
    /// <summary>
    /// Gets a Timeout for the module.
    /// If the module implements <see cref="ITimeoutable"/>, returns the interface timeout.
    /// </summary>
    protected internal virtual TimeSpan Timeout
    {
        get
        {
            if (this is ITimeoutable timeoutable)
            {
                return timeoutable.Timeout;
            }

            return TimeSpan.FromMinutes(30);
        }
    }

    /// <summary>
    /// If true, the pipeline will not fail if this module fails.
    /// If the module implements <see cref="IIgnoreFailures"/>, delegates to the interface.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    /// <returns>A boolean that if true, will stop this Module from failing the pipeline if it fails.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        if (this is IIgnoreFailures ignoreFailures)
        {
            return ignoreFailures.ShouldIgnoreFailures(context, exception);
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Controls whether to skip this module.
    /// If the module implements <see cref="ISkippable"/>, delegates to the interface.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A Skip Decision that controls whether to skip a module, along with a reason.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (this is ISkippable skippable)
        {
            return skippable.ShouldSkip(context);
        }

        return Task.FromResult(SkipDecision.DoNotSkip);
    }

    /// <summary>
    /// If this module is skipped, and this returns true, the result of this module will be reconstructed from the plugged in <see cref="IModuleResultRepository"/>.
    /// If no persisted result can be reconstructed, this module will fail.
    /// If the module implements <see cref="IHistoryAware"/>, delegates to the interface.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A boolean controlling whether to use historical data if available.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context)
    {
        if (this is IHistoryAware historyAware)
        {
            return historyAware.UseResultFromHistoryIfSkipped(context);
        }

        return Task.FromResult(context.ModuleResultRepository.GetType() != typeof(NoOpModuleResultRepository));
    }

    /// <summary>
    /// A hook that runs before the module is started.
    /// If the module implements <see cref="IHookable"/>, delegates to the interface.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task OnBeforeExecute(IPipelineContext context)
    {
        if (this is IHookable hookable)
        {
            return hookable.OnBeforeExecute(context);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// A hook that runs after the module has finished executing.
    /// If the module implements <see cref="IHookable"/>, delegates to the interface.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [ModuleMethodMarker]
    protected internal virtual Task OnAfterExecute(IPipelineContext context)
    {
        if (this is IHookable hookable)
        {
            return hookable.OnAfterExecute(context);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets whether the Module should run even if the pipeline has failed.
    /// If the module implements <see cref="IAlwaysRun"/>, returns AlwaysRun.
    /// </summary>
    public virtual ModuleRunType ModuleRunType
    {
        get
        {
            if (this is IAlwaysRun)
            {
                return ModuleRunType.AlwaysRun;
            }

            return ModuleRunType.OnSuccessfulDependencies;
        }
    }
}
