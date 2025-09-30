using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
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
        Context.Logger.LogError(exception, "[red]✗[/] Module failed after [bold]{Duration}[/]", Module.Duration.ToDisplayString());

        if (IsModuleTimedOutException(exception))
        {
            Context.Logger.LogDebug("Module timed out: {ModuleType}", GetType().FullName);

            Module.Status = Status.TimedOut;
        }
        else if (IsPipelineCanceled(exception))
        {
            Module.Status = Status.PipelineTerminated;
            Context.Logger.LogInformation("[red]⏹[/] Pipeline has been canceled");

            // DON'T throw PipelineCancelledException - just complete the module
            Module.Exception = exception;

            // Set result as cancelled without throwing
            var cancelledResult = new ModuleResult<T>(exception, Module);
            ModuleResultTaskCompletionSource.TrySetResult(cancelledResult);
            return; // Exit without throwing
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
        return exception is TaskCanceledException or OperationCanceledException or ModuleTimeoutException
               && Context.EngineCancellationToken.IsCancelled;
    }

    private bool IsModuleTimedOutException(Exception exception)
    {
        if (Module.Timeout == TimeSpan.Zero)
        {
            return false;
        }

        var isTimeoutExceed = Module.Stopwatch.Elapsed >= Module.Timeout;
        return isTimeoutExceed && exception is ModuleTimeoutException or TaskCanceledException or OperationCanceledException;
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

        var moduleFailedException = new ModuleFailedException(Module, exception);

        // Store the original exception in the cancellation token
        Context.EngineCancellationToken.CancelWithException(moduleFailedException);

        // Throw immediately (no delay)
        ModuleResultTaskCompletionSource.TrySetException(moduleFailedException);
        throw moduleFailedException;
    }
}