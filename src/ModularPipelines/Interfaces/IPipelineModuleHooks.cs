using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Used to hook into before and after individual modules have run
/// </summary>
public interface IPipelineModuleHooks
{
    /// <summary>
    /// A hook to run before a module has started
    /// </summary>
    /// <param name="pipelineContext">A pipeline context object provided by the pipeline.</param>
    /// <param name="module">The module that is due to start.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnBeforeModuleStartAsync(IPipelineHookContext pipelineContext, ModuleBase module);

    /// <summary>
    /// A hook to run after a module has finished
    /// </summary>
    /// <param name="pipelineContext">A pipeline context object provided by the pipeline.</param>
    /// <param name="module">The module that has finished.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task OnAfterModuleEndAsync(IPipelineHookContext pipelineContext, ModuleBase module);
}