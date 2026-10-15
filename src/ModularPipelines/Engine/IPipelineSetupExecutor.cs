using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Invokes globally registered pipeline and module event handlers.
/// </summary>
internal interface IPipelineSetupExecutor
{
    /// <summary>
    /// Gets whether start-handler invocation began, set before awaiting the handlers.
    /// </summary>
    bool PipelineStartInvoked { get; }

    Task OnPipelineStartAsync(CancellationToken cancellationToken);

    Task OnPipelineEndAsync(PipelineSummary pipelineSummary, CancellationToken cancellationToken);

    Task OnModuleReadyAsync(ModuleState moduleState, IConsoleWriter consoleWriter, CancellationToken cancellationToken);

    Task OnModuleStartAsync(ModuleState moduleState, IConsoleWriter consoleWriter, CancellationToken cancellationToken);

    Task OnModuleEndAsync(
        ModuleState moduleState,
        IModuleResult result,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken);

    Task OnModuleFailureAsync(
        ModuleState moduleState,
        Exception exception,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken);

    Task OnModuleSkippedAsync(
        ModuleState moduleState,
        IModuleResult result,
        SkipDecision reason,
        IConsoleWriter consoleWriter,
        CancellationToken cancellationToken);
}
