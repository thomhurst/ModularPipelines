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
    public Status ResolveFailureStatus(IModule module, Exception exception, CancellationToken moduleCancellationToken, CancellationToken pipelineCancellationToken)
    {
        _logger.LogDebug("Resolving failure status for {ModuleName}. Exception Type: {ExceptionType}, Module Canceled: {ModuleCanceled}, Pipeline Canceled: {PipelineCanceled}",
            module.ModuleType.Name, exception.GetType().Name, moduleCancellationToken.IsCancellationRequested, pipelineCancellationToken.IsCancellationRequested);

        // If we have a ModuleTimeoutException, the module timed out
        if (exception is ModuleTimeoutException)
        {
            _logger.LogDebug("Module {ModuleName} timed out", module.ModuleType.Name);
            return Status.TimedOut;
        }

        // If the pipeline was cancelled OR the worker pool was cancelled (StopOnFirstException mode),
        // the module should be marked as PipelineTerminated
        if (pipelineCancellationToken.IsCancellationRequested || moduleCancellationToken.IsCancellationRequested)
        {
            _logger.LogDebug("Module {ModuleName} failed due to cancellation (Pipeline: {PipelineCancelled}, WorkerPool: {WorkerPoolCancelled})",
                module.ModuleType.Name, pipelineCancellationToken.IsCancellationRequested, moduleCancellationToken.IsCancellationRequested);
            return Status.PipelineTerminated;
        }

        // All other exceptions are treated as regular failures
        _logger.LogDebug("Module {ModuleName} failed with {ExceptionType}",
            module.ModuleType.Name, exception.GetType().Name);
        return Status.Failed;
    }
}
