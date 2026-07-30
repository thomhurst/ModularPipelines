using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Polly;

namespace ModularPipelines.Engine;

/// <summary>
/// Orchestrates module execution by applying behaviors based on module configuration.
/// </summary>
/// <remarks>
/// This pipeline reads the module's <see cref="ModuleConfiguration"/> and applies:
/// <list type="bullet">
/// <item>Before/after execution hooks</item>
/// <item>Skip conditions</item>
/// <item>Execution timeout</item>
/// <item>Retry policy</item>
/// <item>Failure handling</item>
/// <item>AlwaysRun behavior (run even on pipeline failure)</item>
/// </list>
/// </remarks>
internal class ModuleExecutionPipeline : IModuleExecutionPipeline
{
    private readonly IModuleResultRepository _resultRepository;
    private readonly IModuleCacheResultRepository? _cacheResultRepository;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IDirectHookInvoker _directHookInvoker;
    private readonly IModuleConditionHandler _moduleConditionHandler;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ModuleExecutionPipeline(
        IModuleResultRepository resultRepository,
        EngineCancellationToken engineCancellationToken,
        IDirectHookInvoker directHookInvoker,
        IModuleConditionHandler moduleConditionHandler,
        IOptions<PipelineOptions> pipelineOptions,
        IModuleCacheResultRepository? cacheResultRepository = null)
    {
        _resultRepository = resultRepository;
        _cacheResultRepository = cacheResultRepository;
        _engineCancellationToken = engineCancellationToken;
        _directHookInvoker = directHookInvoker;
        _moduleConditionHandler = moduleConditionHandler;
        _pipelineOptions = pipelineOptions;
    }

    public async Task<ModuleResult<T>> ExecuteAsync<T>(
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        CancellationToken engineCancellationToken)
    {
        var logger = moduleContext.Logger;
        var moduleName = executionContext.ModuleType.Name;
        ModuleResult<T>? moduleResult = null;
        var beforeHooksExecuted = false;
        var originalCancellationTokenSource = executionContext.ModuleCancellationTokenSource;

        // Get configuration once at the start
        var config = ((IModule) module).Configuration;

        try
        {
            // Setup cancellation based on AlwaysRun behavior
            SetupCancellation(config, executionContext, engineCancellationToken);

            if (config.CacheEnabled && _cacheResultRepository is not null)
            {
                var cachedResult = await TryGetCachedResult(
                        module,
                        moduleContext,
                        logger,
                        executionContext.ModuleCancellationTokenSource.Token)
                    .ConfigureAwait(false);
                if (cachedResult is not null)
                {
                    var cacheHit = SkipDecision.Skip("Module cache hit");
                    await _directHookInvoker.InvokeSkippedAsync(
                            module,
                            moduleContext,
                            cacheHit,
                            executionContext.ModuleCancellationTokenSource.Token)
                        .ConfigureAwait(false);
                    return UseHistoricalResult(
                        module,
                        executionContext,
                        cachedResult,
                        cacheHit,
                        logger,
                        "Using cached module result");
                }
            }

            // A required dependency can skip before this module reaches its own conditions.
            var skipDecision = executionContext.SkipResult;
            if (!skipDecision.ShouldSkip)
            {
                var (shouldIgnore, attributeSkipDecision) = await _moduleConditionHandler
                    .ShouldIgnore(module, executionContext.ModuleCancellationTokenSource.Token)
                    .ConfigureAwait(false);
                if (shouldIgnore)
                {
                    skipDecision = attributeSkipDecision ?? SkipDecision.Skip("Module was ignored");
                }
            }

            if (!skipDecision.ShouldSkip && config.SkipCondition != null)
            {
                skipDecision = await config.SkipCondition(
                        moduleContext,
                        executionContext.ModuleCancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }

            if (skipDecision.ShouldSkip)
            {
                // Call direct skip hook first
                await _directHookInvoker.InvokeSkippedAsync(module, moduleContext, skipDecision, executionContext.ModuleCancellationTokenSource.Token).ConfigureAwait(false);

                return await HandleSkipped(module, executionContext, moduleContext, skipDecision, logger).ConfigureAwait(false);
            }

            // Check for cancellation after skip check
            executionContext.ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            // Execute direct before hook first (virtual override)
            await _directHookInvoker.InvokeBeforeExecuteAsync(module, moduleContext, executionContext.ModuleCancellationTokenSource.Token).ConfigureAwait(false);

            // Track that the before hook executed (for OnAfterExecuteAsync in finally)
            beforeHooksExecuted = true;

            // Mark as processing
            executionContext.Status = Status.Processing;
            executionContext.StartTime = DateTimeOffset.UtcNow;
            executionContext.Stopwatch.Start();

            logger.LogDebug(
                "Module {ModuleName} execution started at {StartTime:O}",
                moduleName,
                executionContext.StartTime);

            // Execute with timeout and retry
            var result = await ExecuteWithPolicies(module, config, executionContext, moduleContext).ConfigureAwait(false);

            // Record successful completion
            executionContext.RecordEndTime();
            executionContext.Status = Status.Successful;

            moduleResult = ModuleResult<T>.CreateSuccess(result, executionContext);

            module.CompletionSource.TrySetResult(moduleResult!);

            // Save to history if applicable
            await SaveResults(
                    module,
                    moduleResult,
                    moduleContext,
                    executionContext.ModuleCancellationTokenSource.Token)
                .ConfigureAwait(false);

            executionContext.SetTypedResult(moduleResult);

            return moduleResult;
        }
        catch (Exception exception)
        {
            executionContext.RecordEndTime();

            // Call direct failed hook (before OnAfterExecuteAsync in finally)
            await _directHookInvoker.InvokeFailedAsync(module, moduleContext, exception, executionContext.ModuleCancellationTokenSource.Token).ConfigureAwait(false);

            // Create the failed result for OnAfterExecuteAsync to receive
            moduleResult = ModuleResult<T>.CreateFailure(exception, executionContext);

            module.CompletionSource.TrySetResult(moduleResult!);

            return await HandleException(module, config, executionContext, moduleContext, exception, logger).ConfigureAwait(false);
        }
        finally
        {
            try
            {
                // Call direct OnAfterExecuteAsync hook - runs on both success and failure
                // Only run if before hooks were executed (module actually started)
                if (beforeHooksExecuted && moduleResult != null)
                {
                    try
                    {
                        var modifiedResult = await _directHookInvoker.InvokeAfterExecuteAsync(module, moduleContext, moduleResult, executionContext.ModuleCancellationTokenSource.Token).ConfigureAwait(false);
                        if (modifiedResult != null)
                        {
                            moduleResult = modifiedResult;
                            executionContext.SetTypedResult(moduleResult);
                        }
                    }
                    catch (Exception afterHookException)
                    {
                        logger.LogError(afterHookException, "Error in OnAfterExecuteAsync hook");
                    }
                }

                LogModuleStatus(executionContext, logger);
            }
            finally
            {
                _cacheResultRepository?.DiscardFingerprint(module);

                var activeCancellationTokenSource = executionContext.ModuleCancellationTokenSource;
                activeCancellationTokenSource.Dispose();

                if (!ReferenceEquals(activeCancellationTokenSource, originalCancellationTokenSource))
                {
                    originalCancellationTokenSource.Dispose();
                }
            }
        }
    }

    private void SetupCancellation(
        ModuleConfiguration config,
        ModuleExecutionContext executionContext,
        CancellationToken engineCancellationToken)
    {
        // AlwaysRun modules don't get cancelled when the engine cancels
        var isAlwaysRun = config.AlwaysRun;
        if (!isAlwaysRun)
        {
            // Create a linked token source that cancels when:
            // - The engine singleton is cancelled (module failures, external cancellation via Ctrl+C or test timeout)
            // - The original module token is cancelled (preserves any existing cancellation on the module)
            // All external cancellation flows through _engineCancellationToken (see ExecutionOrchestrator line 108)
            var originalToken = executionContext.ModuleCancellationTokenSource.Token;
            executionContext.ModuleCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                _engineCancellationToken.Token,
                originalToken);
        }

        executionContext.ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();
    }

    private async Task<ModuleResult<T>> HandleSkipped<T>(
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        SkipDecision skipDecision,
        IModuleLogger logger)
    {
        executionContext.Status = Status.Skipped;
        executionContext.SkipResult = skipDecision;

        // Check if we should use historical data BEFORE setting completion source
        // For skipped modules with a history repository configured, check for cached results
        if (_resultRepository.IsEnabled)
        {
            var historicalResult = await TryGetHistoricalResult(module, moduleContext, logger).ConfigureAwait(false);
            if (historicalResult != null)
            {
                return UseHistoricalResult(
                    module,
                    executionContext,
                    historicalResult,
                    skipDecision,
                    logger,
                    "Using historical result for skipped module");
            }
        }

        var skippedResult = ModuleResult<T>.CreateSkipped(skipDecision, executionContext);
        executionContext.SetTypedResult(skippedResult);
        module.CompletionSource.TrySetResult(skippedResult!);

        logger.LogInformation("Module {ModuleName} skipped: {Reason}",
            executionContext.ModuleType.Name,
            skipDecision.Reason ?? "No reason provided");

        return skippedResult;
    }

    private async Task<ModuleResult<T>?> TryGetHistoricalResult<T>(
        Module<T> module,
        IModuleContext moduleContext,
        IModuleLogger logger)
    {
        try
        {
            return await _resultRepository.GetResultAsync<T>(module, moduleContext).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            logger.LogWarning(
                exception,
                "Could not read a stored result for module {ModuleName}; executing normally",
                module.GetType().Name);
            return null;
        }
    }

    private async Task<ModuleResult<T>?> TryGetCachedResult<T>(
        Module<T> module,
        IModuleContext moduleContext,
        IModuleLogger logger,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _cacheResultRepository!
                .GetResultAsync(module, moduleContext, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            logger.LogWarning(
                exception,
                "Could not read a cached result for module {ModuleName}; executing normally",
                module.GetType().Name);
            return null;
        }
    }

    private static ModuleResult<T> UseHistoricalResult<T>(
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        ModuleResult<T> historicalResult,
        SkipDecision skipDecision,
        IModuleLogger logger,
        string message)
    {
        executionContext.Status = Status.UsedHistory;
        executionContext.SkipResult = skipDecision;
        var usedHistoryResult = historicalResult with { ModuleStatus = Status.UsedHistory };
        executionContext.SetTypedResult(usedHistoryResult);
        module.CompletionSource.TrySetResult(usedHistoryResult);
        logger.LogDebug(message);
        return usedHistoryResult;
    }

    private async Task<T?> ExecuteWithPolicies<T>(
        Module<T> module,
        ModuleConfiguration config,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext)
    {
        var timeout = GetTimeout(config);
        if (config.Timeout is null)
        {
            if (timeout == TimeSpan.Zero)
            {
                moduleContext.Logger.LogDebug("No module timeout configured. The pipeline default timeout is disabled");
            }
            else
            {
                moduleContext.Logger.LogDebug("No module timeout configured. Using pipeline default timeout {Timeout}", timeout);
            }
        }

        var cancellationToken = executionContext.ModuleCancellationTokenSource.Token;

        // Get retry policy if applicable
        var retryPolicy = GetRetryPolicy<T>(config, moduleContext);
        var moduleAttemptRespondedToCancellation = 0;

        async Task<T?> ExecuteModuleAttempt(CancellationToken ct)
        {
            try
            {
                return await module.ExecuteAsync(moduleContext, ct).ConfigureAwait(false);
            }
            finally
            {
                if (ct.IsCancellationRequested)
                {
                    Volatile.Write(ref moduleAttemptRespondedToCancellation, 1);
                }
            }
        }

        // Create the execution function that optionally includes retry
        Func<CancellationToken, Task<T?>> executeFunc = retryPolicy != null
            ? ct => retryPolicy.ExecuteAsync(ExecuteModuleAttempt, ct)
            : ct => module.ExecuteAsync(moduleContext, ct);

        // Use TimeoutHelper with detailed results to get information about token cooperation
        var timeoutResult = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            executeFunc,
            timeout == TimeSpan.Zero ? null : timeout,
            cancellationToken,
            $"Module {executionContext.ModuleType.Name} timed out after {timeout}").ConfigureAwait(false);

        if (timeoutResult.TimedOut)
        {
            var wasCancellationTokenRespected = timeoutResult.WasCancellationTokenRespected
                                                && (retryPolicy is null
                                                    || Volatile.Read(ref moduleAttemptRespondedToCancellation) == 1);

            // Create a detailed timeout exception with information about token cooperation
            throw new ModuleTimeoutException(
                executionContext.ModuleType,
                timeout,
                timeoutResult.ElapsedTime,
                wasCancellationTokenRespected);
        }

        return timeoutResult.Value;
    }

    private TimeSpan GetTimeout(ModuleConfiguration config)
    {
        return config.Timeout ?? _pipelineOptions.Value.DefaultModuleTimeout;
    }

    private static IAsyncPolicy? GetRetryPolicy<T>(
        ModuleConfiguration config,
        IModuleContext moduleContext)
    {
        if (config.AdvancedRetryPolicyFactory != null)
        {
            return config.AdvancedRetryPolicyFactory(moduleContext);
        }

        if (config.RetryConfiguration != null)
        {
            return ModuleRetryPolicyFactory.Create(config.RetryConfiguration);
        }

        // Check if default retry count is configured
        var defaultRetryCount = moduleContext.Services.Options.DefaultRetryCount;
        if (defaultRetryCount > 0)
        {
            return ModuleRetryPolicyFactory.Create(new ModuleRetryConfiguration(
                defaultRetryCount,
                ModuleRetryConfiguration.DefaultBaseDelay,
                ShouldRetry: null));
        }

        return null;
    }

    private async Task SaveResults<T>(
        Module<T> module,
        ModuleResult<T> result,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        if (_resultRepository.IsEnabled)
        {
            try
            {
                await _resultRepository.SaveResultAsync(module, result, moduleContext).ConfigureAwait(false);
            }
            catch (Exception e) when (e is not (OutOfMemoryException or StackOverflowException))
            {
                moduleContext.Logger.LogError(e, "Error saving module result to repository");
            }
        }

        if (!((IModule) module).Configuration.CacheEnabled || _cacheResultRepository is null)
        {
            return;
        }

        try
        {
            await _cacheResultRepository
                .SaveResultAsync(module, result, moduleContext, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            moduleContext.Logger.LogDebug(
                "Module cache save canceled for module {ModuleName}",
                module.GetType().Name);
        }
        catch (Exception e) when (e is not (OutOfMemoryException or StackOverflowException))
        {
            moduleContext.Logger.LogError(e, "Error saving module result to module cache");
        }
    }

    private async Task<ModuleResult<T>> HandleException<T>(
        Module<T> module,
        ModuleConfiguration config,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        Exception exception,
        IModuleLogger logger)
    {
        logger.LogError(exception, "Module failed after {Duration}", executionContext.Duration.ToDisplayString());

        executionContext.Exception = exception;

        // Check for timeout - use the enhanced exception type for detailed logging
        if (exception is ModuleTimeoutException timeoutException)
        {
            executionContext.Status = Status.TimedOut;

            // Log additional timeout details
            if (!timeoutException.WasCancellationTokenRespected)
            {
                logger.LogWarning(
                    "Module {ModuleName} did not complete within the cancellation grace period; timeout enforcement stopped waiting after {ElapsedTime}",
                    executionContext.ModuleType.Name,
                    timeoutException.ElapsedTime.ToDisplayString());
            }
        }
        else if (IsTimeout(config, executionContext, exception))
        {
            executionContext.Status = Status.TimedOut;
        }

        // Check for pipeline cancellation
        else if (IsPipelineCancelled(exception))
        {
            executionContext.Status = Status.PipelineTerminated;
            logger.LogInformation("Pipeline has been canceled");

            var cancelledResult = ModuleResult<T>.CreateFailure(exception, executionContext);
            executionContext.SetTypedResult(cancelledResult);
            return cancelledResult;
        }
        else
        {
            executionContext.Status = Status.Failed;
        }

        // Check if we should ignore failures
        if (config.IgnoreFailuresCondition != null)
        {
            if (await config.IgnoreFailuresCondition(moduleContext, exception).ConfigureAwait(false))
            {
                logger.LogDebug("Ignoring failures in this module and continuing...");
                executionContext.Status = Status.IgnoredFailure;

                var ignoredResult = ModuleResult<T>.CreateFailure(exception, executionContext);
                executionContext.SetTypedResult(ignoredResult);

                await SaveResults(
                        module,
                        ignoredResult,
                        moduleContext,
                        executionContext.ModuleCancellationTokenSource.Token)
                    .ConfigureAwait(false);
                return ignoredResult;
            }
        }

        // Create a failed result before cancelling and throwing
        ModuleResult<T> failedResult = ModuleResult<T>.CreateFailure(exception, executionContext);
        executionContext.SetTypedResult(failedResult);

        // Cancel the pipeline and propagate
        CancelPipelineAndThrow(executionContext, moduleContext, exception, logger);

        // This won't be reached, but compiler needs it
        throw exception;
    }

    private bool IsTimeout(ModuleConfiguration config, ModuleExecutionContext executionContext, Exception exception)
    {
        var timeout = GetTimeout(config);
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
        ((IInternalModuleLogger) logger).SetException(exception);

        var moduleFailedException = new ModuleFailedException(executionContext.ModuleType, exception);

        if (moduleContext.Services.Options.ExecutionMode == ExecutionMode.StopOnFirstException)
        {
            logger.LogDebug("Module failed. Cancelling the pipeline");
            _engineCancellationToken.CancelWithException(moduleFailedException);
        }
        else
        {
            logger.LogDebug("Module failed. Waiting for all modules to complete");
            _engineCancellationToken.RecordException(moduleFailedException);
        }

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
            Status.Skipped => LogLevel.Information,
            Status.Unknown => LogLevel.Error,
            Status.IgnoredFailure => LogLevel.Warning,
            Status.PipelineTerminated => LogLevel.Error,
            Status.UsedHistory => LogLevel.Information,
            Status.Retried => LogLevel.Warning,
            _ => LogLevel.Error,
        };

        logger.Log(logLevel, message);
    }
}
