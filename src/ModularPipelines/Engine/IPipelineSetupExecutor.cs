using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnStartAsync();

    Task OnEndAsync(PipelineSummary pipelineSummary);

    Task OnBeforeModuleStartAsync(IModule module);

    Task OnAfterModuleEndAsync(IModule module);
}
