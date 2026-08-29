using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnPipelineStartAsync();

    Task OnPipelineEndAsync(PipelineSummary pipelineSummary);

    Task OnModuleReadyAsync(ModuleState moduleState, IConsoleWriter consoleWriter);

    Task OnModuleStartAsync(ModuleState moduleState);

    Task OnModuleEndAsync(ModuleState moduleState, IModuleResult result);

    Task OnModuleFailureAsync(ModuleState moduleState, Exception exception);

    Task OnModuleSkippedAsync(ModuleState moduleState, SkipDecision reason);
}
