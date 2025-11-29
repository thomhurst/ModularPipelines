using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnStartAsync();

    Task OnEndAsync(PipelineSummary pipelineSummary);

    Task OnBeforeModuleStartAsync(ModuleState moduleState);

    Task OnAfterModuleEndAsync(ModuleState moduleState);
}