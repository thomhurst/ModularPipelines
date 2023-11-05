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
            return;
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
            await CancelPipelineAndThrow(exception);
        }
    }

    private bool IsPipelineCanceled(Exception exception)
    {
        return exception is TaskCanceledException or OperationCanceledException
               && Context.EngineCancellationToken.IsCancellationRequested;
    }

    private bool IsModuleTimedOutExeption(Exception exception)
    {
        return exception is TaskCanceledException or OperationCanceledException
               && ModuleCancellationTokenSource.IsCancellationRequested
               && !Context.EngineCancellationToken.IsCancellationRequested;
    }

    private async Task SaveFailedResult(Exception exception)
    {
        Context.Logger.LogDebug("Ignoring failures in this module and continuing...");

        var moduleResult = new ModuleResult<T>(exception, Module);

        Module.Status = Status.IgnoredFailure;

        await Module.HistoryHandler.SaveResult(moduleResult);
    }

    private async Task CancelPipelineAndThrow(Exception exception)
    {
        Context.Logger.LogDebug("Module failed. Cancelling the pipeline");

        Context.EngineCancellationToken.CancelWithReason($"{Module.GetType().Name} failed with a {exception.GetType().Name}");

        // Time for cancellation to register
        await Task.Delay(TimeSpan.FromMilliseconds(200));

        ModuleResultTaskCompletionSource.TrySetException(exception);
        
        Context.Logger.SetException(exception);

        throw new ModuleFailedException(Module, exception);
    }
}