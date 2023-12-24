using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class ErrorHandler<T> : BaseHandler<T>, IErrorHandler
{
    public ErrorHandler(Module<T> module) : base(module)
    {
    }

    public async Task Handle(Exception exception)
    {
        Context.Logger.LogError(exception, "Module Failed after {Duration}", Module.Duration);

        if (IsModuleTimedOutExeption(exception))
        {
            Context.Logger.LogDebug("Module timed out: {ModuleType}", GetType().FullName);

            Module.Status = Status.TimedOut;
        }
        else if (IsPipelineCanceled(exception))
        {
            Module.Status = Status.PipelineTerminated;
            Context.Logger.LogInformation("Pipeline has been canceled");
            
            // This shouldn't throw back to the main thread as the initial failing module should be the first to throw
            // So give 2 seconds to let that happen first, and then throw here as a safety net to ensure the process fails
            await Task.Delay(TimeSpan.FromSeconds(2));
            
            throw new PipelineCancelledException(Context.EngineCancellationToken);
        }
        else
        {
            Module.Status = Status.Failed;
        }

        Module.Exception = exception;

        if (await Module.ShouldIgnoreFailures(Context, exception))
        {
            await SaveFailedResult(exception);
        }
        else
        {
            CancelPipelineAndThrow(exception);
        }
    }

    private bool IsPipelineCanceled(Exception exception)
    {
        return exception is TaskCanceledException or OperationCanceledException
               && Context.EngineCancellationToken.IsCancelled;
    }

    private bool IsModuleTimedOutExeption(Exception exception)
    {
        return exception is TaskCanceledException or OperationCanceledException
               && ModuleCancellationTokenSource.IsCancellationRequested
               && !Context.EngineCancellationToken.IsCancelled;
    }

    private async Task SaveFailedResult(Exception exception)
    {
        Context.Logger.LogDebug("Ignoring failures in this module and continuing...");

        var moduleResult = new ModuleResult<T>(exception, Module);

        Module.Status = Status.IgnoredFailure;

        await Module.HistoryHandler.SaveResult(moduleResult);
    }

    private void CancelPipelineAndThrow(Exception exception)
    {
        Context.Logger.LogDebug("Module failed. Cancelling the pipeline");

        Context.Logger.SetException(exception);

        _ = CancelPipeline(exception);

        ModuleResultTaskCompletionSource.TrySetException(exception);
        
        throw new ModuleFailedException(Module, exception);
    }

    private async Task CancelPipeline(Exception exception)
    {
        // Cancel after a second, so other modules don't fail before this one.
        await Task.Delay(TimeSpan.FromSeconds(1));

        Context.EngineCancellationToken.CancelWithReason(
            $"{Module.GetType().Name} failed with a {exception.GetType().Name}"
        );
    }
}