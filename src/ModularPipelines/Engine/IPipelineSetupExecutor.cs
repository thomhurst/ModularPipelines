using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnStartAsync();

    Task OnEndAsync(PipelineSummary pipelineSummary);

    Task OnBeforeModuleStartAsync(ModuleBase module);

    Task OnAfterModuleEndAsync(ModuleBase module);
}
