using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnPipelineStartAsync();

    Task OnPipelineEndAsync(PipelineSummary pipelineSummary);

    Task OnModuleReadyAsync(ModuleState moduleState);

    Task OnModuleStartAsync(ModuleState moduleState);

    Task OnModuleEndAsync(ModuleState moduleState);

    Task OnModuleFailureAsync(ModuleState moduleState);

    Task OnModuleSkippedAsync(ModuleState moduleState);
}
