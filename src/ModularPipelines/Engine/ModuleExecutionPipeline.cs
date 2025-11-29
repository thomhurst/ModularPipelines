using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using Polly;

namespace ModularPipelines.Engine;

/// <summary>
/// Interface for the module execution pipeline.
/// </summary>
internal interface IModuleExecutionPipeline
{
    /// <summary>
    /// Executes a module with all applicable behaviors.
    /// </summary>
    Task<ModuleResult<T>> ExecuteAsync<T>(
        IModule<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        CancellationToken engineCancellationToken);
}

/// <summary>
/// Orchestrates module execution by applying behaviors based on implemented interfaces.
/// </summary>
/// <remarks>
/// This pipeline checks which behavior interfaces a module implements and applies them:
/// <list type="bullet">
/// <item><see cref="IHookable"/> - Before/after hooks</item>
/// <item><see cref="ISkippable"/> - Skip conditions</item>
/// <item><see cref="ITimeoutable"/> - Execution timeout</item>
/// <item><see cref="IRetryable{T}"/> - Retry policy</item>
/// <item><see cref="IIgnoreFailures"/> - Failure handling</item>
/// <item><see cref="IHistoryAware"/> - Historical result caching</item>
/// <item><see cref="IAlwaysRun"/> - Run even on pipeline failure</item>
/// </list>
/// </remarks>
internal class ModuleExecutionPipeline : IModuleExecutionPipeline
{
    private const int DefaultTimeoutMinutes = 30;

    private readonly IModuleResultRepository _resultRepository;
    private readonly EngineCancellationToken _engineCancellationToken;

    public ModuleExecutionPipeline(
        IModuleResultRepository resultRepository,
        EngineCancellationToken engineCancellationToken)
    {
        _resultRepository = resultRepository;
        _engineCancellationToken = engineCancellationToken;
    }

    public async Task<ModuleResult<T>> ExecuteAsync<T>(
        IModule<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        CancellationToken engineCancellationToken)
    {
        var logger = moduleContext.Logger;
        var moduleName = executionContext.ModuleType.Name;

        try
        {
            // Setup cancellation based on AlwaysRun behavior
            SetupCancellation(module, executionContext, engineCancellationToken);

            // Check for skip condition
            if (module is ISkippable skippable)
            {
                var skipDecision = await skippable.ShouldSkip(moduleContext);
                if (skipDecision.ShouldSkip)
                {
                    return await HandleSkipped(module, executionContext, moduleContext, skipDecision, logger);
                }
            }

            // Check for cancellation after skip check
            executionContext.ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            // Execute before hook
            if (module is IHookable hookable)
            {
                await hookable.OnBeforeExecute(moduleContext);
            }

            // Mark as processing
            executionContext.Status = Status.Processing;
            executionContext.StartTime = DateTimeOffset.UtcNow;
            executionContext.Stopwatch.Start();

            logger.LogDebug("Module {ModuleName} execution started at {StartTime}", moduleName, executionContext.StartTime);

            // Execute with timeout and retry
            var result = await ExecuteWithPolicies(module, executionContext, moduleContext);

            // Record successful completion
            executionContext.RecordEndTime();
            executionContext.Status = Status.Successful;

            var moduleResult = new ModuleResult<T>(result, executionContext);

            // Save to history if applicable
            await SaveToHistory(module, moduleResult, moduleContext);

            logger.LogDebug("Module succeeded after {Duration}", executionContext.Duration);

            executionContext.SetTypedResult(moduleResult);
            return moduleResult;
        }
        catch (Exception exception)
        {
            executionContext.RecordEndTime();
            return await HandleException(module, executionContext, moduleContext, exception, logger);
        }
        finally
        {
            // Execute after hook
            if (module is IHookable hookable)
            {
                try
                {
                    await hookable.OnAfterExecute(moduleContext);
                }
                catch (Exception hookException)
                {
                    logger.LogError(hookException, "Error in OnAfterExecute hook");
                }
            }

            LogModuleStatus(executionContext, logger);
        }
    }

    private void SetupCancellation(
        IModule module,
        ModuleExecutionContext executionContext,
        CancellationToken engineCancellationToken)
    {
        // AlwaysRun modules don't get cancelled when the engine cancels
        if (module is not IAlwaysRun)
        {
            engineCancellationToken.Register(() =>
                executionContext.ModuleCancellationTokenSource.Cancel());
        }

        executionContext.ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();
    }

    private async Task<ModuleResult<T>> HandleSkipped<T>(
        IModule<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        SkipDecision skipDecision,
        IModuleLogger logger)
    {
        executionContext.Status = Status.Skipped;
        executionContext.SkipResult = skipDecision;

        // Check if we should use historical data
        if (module is IHistoryAware historyAware)
        {
            if (await historyAware.UseResultFromHistoryIfSkipped(moduleContext))
            {
                var historicalResult = await _resultRepository.GetResultAsync<T>(module, moduleContext);
                if (historicalResult != null)
                {
                    executionContext.Status = Status.UsedHistory;
                    executionContext.SetTypedResult(historicalResult);
                    logger.LogDebug("Using historical result for skipped module");
                    return historicalResult;
                }
            }
        }

        var skippedResult = new SkippedModuleResult<T>(executionContext, skipDecision);
        executionContext.SetTypedResult(skippedResult);

        logger.LogInformation("Module {ModuleName} skipped: {Reason}",
            executionContext.ModuleType.Name,
            skipDecision.Reason ?? "No reason provided");

        return skippedResult;
    }

    private async Task<T?> ExecuteWithPolicies<T>(
        IModule<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext)
    {
        var timeout = GetTimeout(module);
        var cancellationToken = executionContext.ModuleCancellationTokenSource.Token;

        // Setup timeout
        if (timeout != TimeSpan.Zero)
        {
            executionContext.ModuleCancellationTokenSource.CancelAfter(timeout);
        }

        // Get retry policy if applicable
        var retryPolicy = GetRetryPolicy(module, moduleContext);

        // Execute with policies
        var executeTask = retryPolicy != null
            ? retryPolicy.ExecuteAsync(() => module.ExecuteAsync(moduleContext, cancellationToken))
            : module.ExecuteAsync(moduleContext, cancellationToken);

        if (timeout != TimeSpan.Zero)
        {
            // Race against timeout
            var timeoutTask = Task.Delay(timeout, cancellationToken);

            var completedTask = await Task.WhenAny(executeTask, timeoutTask);

            if (completedTask == timeoutTask && executionContext.Status != Status.Successful)
            {
                throw new ModuleTimeoutException(executionContext.ModuleType, timeout);
            }
        }

        return await executeTask;
    }

    private TimeSpan GetTimeout(IModule module)
    {
        if (module is ITimeoutable timeoutable)
        {
            return timeoutable.Timeout;
        }

        return TimeSpan.FromMinutes(DefaultTimeoutMinutes);
    }

    private static Polly.Retry.AsyncRetryPolicy<T?>? GetRetryPolicy<T>(
        IModule<T> module,
        IModuleContext moduleContext)
    {
        if (module is IRetryable<T> retryable)
        {
            return retryable.GetRetryPolicy(moduleContext);
        }

        return null;
    }

    private async Task SaveToHistory<T>(
        IModule<T> module,
        ModuleResult<T> result,
        IModuleContext moduleContext)
    {
        if (_resultRepository.GetType() == typeof(NoOpModuleResultRepository))
        {
            return;
        }

        try
        {
            await _resultRepository.SaveResultAsync(module, result, moduleContext);
        }
        catch (Exception e)
        {
            moduleContext.Logger.LogError(e, "Error saving module result to repository");
        }
    }

    private async Task<ModuleResult<T>> HandleException<T>(
        IModule<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        Exception exception,
        IModuleLogger logger)
    {
        logger.LogError(exception, "Module failed after {Duration}", executionContext.Duration.ToDisplayString());

        executionContext.Exception = exception;

        // Check for timeout
        if (IsTimeout(module, executionContext, exception))
        {
            executionContext.Status = Status.TimedOut;
        }
        // Check for pipeline cancellation
        else if (IsPipelineCancelled(exception))
        {
            executionContext.Status = Status.PipelineTerminated;
            logger.LogInformation("Pipeline has been canceled");

            var cancelledResult = new ModuleResult<T>(exception, executionContext);
            executionContext.SetTypedResult(cancelledResult);
            return cancelledResult;
        }
        else
        {
            executionContext.Status = Status.Failed;
        }

        // Check if we should ignore failures
        if (module is IIgnoreFailures ignoreFailures)
        {
            if (await ignoreFailures.ShouldIgnoreFailures(moduleContext, exception))
            {
                logger.LogDebug("Ignoring failures in this module and continuing...");
                executionContext.Status = Status.IgnoredFailure;

                var ignoredResult = new ModuleResult<T>(exception, executionContext);
                executionContext.SetTypedResult(ignoredResult);

                await SaveToHistory(module, ignoredResult, moduleContext);
                return ignoredResult;
            }
        }

        // Cancel the pipeline and propagate
        CancelPipelineAndThrow(executionContext, moduleContext, exception, logger);

        // This won't be reached, but compiler needs it
        throw exception;
    }

    private bool IsTimeout(IModule module, ModuleExecutionContext executionContext, Exception exception)
    {
        var timeout = GetTimeout(module);
        if (timeout == TimeSpan.Zero)
        {
            return false;
        }

        var isTimeoutExceeded = executionContext.Stopwatch.Elapsed >= timeout;
        return isTimeoutExceeded && exception is ModuleTimeoutException or TaskCanceledException or OperationCanceledException;
    }

    private bool IsPipelineCancelled(Exception exception)
    {
        return exception is TaskCanceledException or OperationCanceledException or ModuleTimeoutException
               && _engineCancellationToken.IsCancelled;
    }

    private void CancelPipelineAndThrow(
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        Exception exception,
        IModuleLogger logger)
    {
        logger.LogDebug("Module failed. Cancelling the pipeline");
        logger.SetException(exception);

        var moduleFailedException = new ModuleFailedException(executionContext.ModuleType, exception);

        _engineCancellationToken.CancelWithException(moduleFailedException);

        executionContext.SetException(moduleFailedException);
        throw moduleFailedException;
    }

    private static void LogModuleStatus(ModuleExecutionContext executionContext, IModuleLogger logger)
    {
        var moduleName = executionContext.ModuleType.Name;
        var message = StatusDisplayProvider.FormatStatusMessage(moduleName, executionContext.Status);

        var logLevel = executionContext.Status switch
        {
            Status.NotYetStarted => LogLevel.Warning,
            Status.Processing => LogLevel.Error,
            Status.Successful => LogLevel.Information,
            Status.Failed => LogLevel.Error,
            Status.TimedOut => LogLevel.Error,
            Status.Skipped => LogLevel.Warning,
            Status.Unknown => LogLevel.Error,
            Status.IgnoredFailure => LogLevel.Warning,
            Status.PipelineTerminated => LogLevel.Error,
            Status.UsedHistory => LogLevel.Information,
            Status.Retried => LogLevel.Warning,
            _ => LogLevel.Error
        };

        logger.Log(logLevel, message);
    }
}
