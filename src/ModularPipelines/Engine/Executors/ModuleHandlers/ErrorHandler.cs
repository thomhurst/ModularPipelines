using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class ErrorHandler<T> : BaseHandler<T>, IErrorHandler<T>
{
    private readonly ILogger<ErrorHandler<T>> _logger;

    public ErrorHandler(Module<T> module) : base(module)
    {
        _logger = module.Context.Get<ILogger<ErrorHandler<T>>>()!;
    }

    public async Task<ModuleResult<T>> Handle(Exception exception)
    {
        _logger.LogDebug("{Module} has thrown {Exception}", Module.GetType().Name, exception.GetType().Name);
        
        Context.Logger.LogError(exception, "Module Failed after {Duration}", Module.Duration);

        if (IsModuleTimedOutException(exception))
        {
            _logger.LogDebug("{Module} has timed out", Module.GetType().Name);
            
            Context.Logger.LogDebug("Module timed out: {ModuleType}", GetType().FullName);

            Module.Status = Status.TimedOut;
        }
        else if (IsPipelineCanceled(exception))
        {
            _logger.LogDebug("{Module} has terminated because of pipeline cancellation", Module.GetType().Name);
            
            Module.Status = Status.PipelineTerminated;
            Context.Logger.LogInformation("Pipeline has been canceled");

            throw new PipelineCancelledException(Context.EngineCancellationToken);
        }
        else
        {
            Module.Status = Status.Failed;
        }

        Module.Exception = exception;

        if (await Module.ShouldIgnoreFailures(Context, exception))
        {
            _logger.LogDebug("{Module} is ignoring its exception and continuing the pipeline", Module.GetType().Name);
            
            return await SaveFailedResult(exception);
        }

        throw CancelPipelineAndThrow(exception);
    }

    private bool IsPipelineCanceled(Exception exception)
    {
        return exception is TaskCanceledException or OperationCanceledException
               && Context.EngineCancellationToken.IsCancelled;
    }

    private bool IsModuleTimedOutException(Exception exception)
    {
        return exception is ModuleTimeoutException or TaskCanceledException or OperationCanceledException
               && ModuleCancellationTokenSource.IsCancellationRequested
               && !Context.EngineCancellationToken.IsCancelled;
    }

    private async Task<ModuleResult<T>> SaveFailedResult(Exception exception)
    {
        Context.Logger.LogDebug("Ignoring failures in this module and continuing...");

        var moduleResult = new ModuleResult<T>(exception, Module);

        Module.Status = Status.IgnoredFailure;

        await Module.HistoryHandler.SaveResult(moduleResult);

        return moduleResult;
    }

    [DoesNotReturn]
    private Exception CancelPipelineAndThrow(Exception exception)
    {
        _logger.LogDebug("{Module} is throwing its exception", Module.GetType().Name);
        
        Context.Logger.LogDebug("Module failed. Cancelling the pipeline");

        Context.Logger.SetException(exception);
        
        Context.EngineCancellationToken.Cancel();
        
        throw new ModuleFailedException(Module, exception);
    }
}