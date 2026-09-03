using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal interface IPipelineSetupExecutor
{
    Task OnPipelineStartAsync();

    Task OnPipelineEndAsync(PipelineSummary pipelineSummary);

    Task OnModuleReadyAsync(ModuleState moduleState, IConsoleWriter consoleWriter);

    Task OnModuleStartAsync(ModuleState moduleState, IConsoleWriter consoleWriter);

    Task OnModuleEndAsync(ModuleState moduleState, IModuleResult result, IConsoleWriter consoleWriter);

    Task OnModuleFailureAsync(ModuleState moduleState, Exception exception, IConsoleWriter consoleWriter);

    Task OnModuleSkippedAsync(ModuleState moduleState, SkipDecision reason, IConsoleWriter consoleWriter);
}
