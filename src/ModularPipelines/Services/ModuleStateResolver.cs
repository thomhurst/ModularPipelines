using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Resolves the final status of a module based on execution results and pipeline state.
/// </summary>
internal class ModuleStateResolver : IModuleStateResolver
{
    private readonly ILogger<ModuleStateResolver> _logger;

    public ModuleStateResolver(ILogger<ModuleStateResolver> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Status ResolveFailureStatus(IModule module, Exception exception, CancellationToken pipelineCancellationToken)
    {
        // If the pipeline was cancelled, the module should always be marked as PipelineTerminated
        // regardless of the exception type
        if (pipelineCancellationToken.IsCancellationRequested)
        {
            _logger.LogDebug("Module {ModuleName} failed due to pipeline cancellation", module.ModuleType.Name);
            return Status.PipelineTerminated;
        }

        // If we have a ModuleTimeoutException and pipeline wasn't cancelled, the module timed out
        if (exception is ModuleTimeoutException)
        {
            _logger.LogDebug("Module {ModuleName} timed out", module.ModuleType.Name);
            return Status.TimedOut;
        }

        // All other exceptions are treated as regular failures
        _logger.LogDebug("Module {ModuleName} failed with {ExceptionType}",
            module.ModuleType.Name, exception.GetType().Name);
        return Status.Failed;
    }
}
