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
using ModularPipelines.Tracing;
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
        CancellationToken engineCancellationToken,
        Func<CancellationToken, Task>? prepareExecutionAsync = null,
        Func<ModuleResult<T>, CancellationToken, Task>? finalizeExecutionAsync = null,
        bool completeModule = true)
    {
        var logger = moduleContext.Logger;
        var moduleName = executionContext.ModuleType.Name;
        ModuleResult<T>? moduleResult = null;
        var beforeHooksExecuted = false;
        var afterHookInvoked = false;
        var originalCancellationTokenSource = executionContext.ModuleCancellationTokenSource;
        var finalizer = new ModuleExecutionFinalizer<T>(
            module,
            executionContext,
            finalizeExecutionAsync,
            completeModule);

        // Get configuration once at the start
        var config = ((IModule) module).Configuration;

        try
        {
            // Setup cancellation based on AlwaysRun behavior
            SetupCancellation(config, executionContext, engineCancellationToken);

            var skipDecision = await GetSkipDecisionAsync(
                    module,
                    config,
                    executionContext,
                    moduleContext)
                .ConfigureAwait(false);
            if (skipDecision.ShouldSkip)
            {
                // Call direct skip hook first
                await _directHookInvoker.InvokeSkippedAsync(module, moduleContext, skipDecision, executionContext.ModuleCancellationTokenSource.Token).ConfigureAwait(false);

                var skippedResult = await HandleSkipped(
                        module,
                        executionContext,
                        moduleContext,
                        skipDecision,
                        logger)
                    .ConfigureAwait(false);
                await finalizer.FinalizeAsync(skippedResult).ConfigureAwait(false);
                executionContext.SetTypedResult(skippedResult);
                finalizer.Complete(skippedResult);

                return skippedResult;
            }

            // Check for cancellation after skip check
            executionContext.ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();
            if (prepareExecutionAsync is not null)
            {
                await prepareExecutionAsync(executionContext.ModuleCancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }

            var cachedResult = await TryUseCachedResultAsync(
                    module,
                    config,
                    executionContext,
                    moduleContext,
                    logger)
                .ConfigureAwait(false);
            if (cachedResult is not null)
            {
                await finalizer.FinalizeAsync(cachedResult).ConfigureAwait(false);
                executionContext.SetTypedResult(cachedResult);
                finalizer.Complete(cachedResult);

                return cachedResult;
            }

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

            afterHookInvoked = true;
            moduleResult = await InvokeAfterExecuteAsync(
                    module,
                    moduleContext,
                    moduleResult,
                    executionContext.ModuleCancellationTokenSource.Token)
                .ConfigureAwait(false);

            await finalizer.FinalizeAsync(moduleResult).ConfigureAwait(false);

            executionContext.SetTypedResult(moduleResult);
            finalizer.Complete(moduleResult);

            // Save to history if applicable
            await SaveResults(
                    module,
                    moduleResult,
                    moduleContext,
                    executionContext.ModuleCancellationTokenSource.Token)
                .ConfigureAwait(false);

            return moduleResult;
        }
        catch (Exception exception) when (!finalizer.WasInvoked)
        {
            executionContext.RecordEndTime();

            // Call direct failed hook (before OnAfterExecuteAsync in finally)
            await _directHookInvoker.InvokeFailedAsync(module, moduleContext, exception, executionContext.ModuleCancellationTokenSource.Token).ConfigureAwait(false);

            moduleResult = await HandleException(
                    module,
                    config,
                    executionContext,
                    moduleContext,
                    exception,
                    logger,
                    result => moduleResult = result,
                    completeModule)
                .ConfigureAwait(false);
            await finalizer.FinalizeAsync(moduleResult).ConfigureAwait(false);
            finalizer.Complete(moduleResult);

            return moduleResult;
        }
        finally
        {
            try
            {
                moduleResult = await InvokePendingAfterHookAsync(
                        module,
                        moduleContext,
                        executionContext,
                        moduleResult,
                        beforeHooksExecuted,
                        afterHookInvoked)
                    .ConfigureAwait(false);

                LogModuleStatus(executionContext, logger);
            }
            finally
            {
                _cacheResultRepository?.DiscardFingerprint(module);
                DisposeCancellationTokenSources(executionContext, originalCancellationTokenSource);
            }
        }
    }

    private async Task<ModuleResult<T>?> InvokePendingAfterHookAsync<T>(
        Module<T> module,
        IModuleContext moduleContext,
        ModuleExecutionContext<T> executionContext,
        ModuleResult<T>? moduleResult,
        bool beforeHooksExecuted,
        bool afterHookInvoked)
    {
        if (!beforeHooksExecuted || moduleResult is null || afterHookInvoked)
        {
            return moduleResult;
        }

        moduleResult = await InvokeAfterExecuteAsync(
                module,
                moduleContext,
                moduleResult,
                CancellationToken.None)
            .ConfigureAwait(false);
        executionContext.SetTypedResult(moduleResult);
        return moduleResult;
    }

    private static void DisposeCancellationTokenSources(
        ModuleExecutionContext executionContext,
        CancellationTokenSource originalCancellationTokenSource)
    {
        var activeCancellationTokenSource = executionContext.ModuleCancellationTokenSource;
        activeCancellationTokenSource.Dispose();

        if (!ReferenceEquals(activeCancellationTokenSource, originalCancellationTokenSource))
        {
            originalCancellationTokenSource.Dispose();
        }
    }

    private async Task<SkipDecision> GetSkipDecisionAsync<T>(
        Module<T> module,
        ModuleConfiguration config,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext)
    {
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

        if (!skipDecision.ShouldSkip && config.SkipCondition is not null)
        {
            skipDecision = await config.SkipCondition(
                    moduleContext,
                    executionContext.ModuleCancellationTokenSource.Token)
                .ConfigureAwait(false);
        }

        return skipDecision;
    }

    private async Task<ModuleResult<T>?> TryUseCachedResultAsync<T>(
        Module<T> module,
        ModuleConfiguration config,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        IModuleLogger logger)
    {
        if (_pipelineOptions.Value.DisableModuleCache
            || !config.CacheEnabled
            || _cacheResultRepository is null)
        {
            ModuleActivityTracing.RecordCacheDisabled(executionContext.ModuleActivity);
            return null;
        }

        var cancellationToken = executionContext.ModuleCancellationTokenSource.Token;
        var cachedResult = await TryGetCachedResult(
                module,
                moduleContext,
                logger,
                cancellationToken)
            .ConfigureAwait(false);
        if (cachedResult is null)
        {
            ModuleActivityTracing.RecordCacheMiss(executionContext.ModuleActivity, module.GetType());
            return null;
        }

        var result = UseCachedResult(
            executionContext,
            cachedResult,
            logger);
        ModuleActivityTracing.RecordCacheHit(executionContext.ModuleActivity, module.GetType());
        await _directHookInvoker.InvokeCachedResultAsync(
                module,
                moduleContext,
                result,
                cancellationToken)
            .ConfigureAwait(false);
        return result;
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
            // - The execution caller is cancelled
            // - The original module token is cancelled (preserves any existing cancellation on the module)
            // Pipeline-wide external cancellation flows through _engineCancellationToken
            // (see ExecutionOrchestrator line 108).
            var originalToken = executionContext.ModuleCancellationTokenSource.Token;
            executionContext.ModuleCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                _engineCancellationToken.Token,
                engineCancellationToken,
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
        if (_resultRepository.IsEnabled
            && executionContext.AllowHistoricalResultWhenSkipped)
        {
            var historicalResult = await TryGetHistoricalResult(module, moduleContext, logger).ConfigureAwait(false);
            if (historicalResult != null)
            {
                return UseHistoricalResult(
                    executionContext,
                    historicalResult,
                    skipDecision,
                    logger,
                    "Using historical result for skipped module");
            }
        }

        var skippedResult = ModuleResult<T>.CreateSkipped(skipDecision, executionContext);

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
        ModuleExecutionContext<T> executionContext,
        ModuleResult<T> historicalResult,
        SkipDecision skipDecision,
        IModuleLogger logger,
        string message)
    {
        executionContext.Status = Status.UsedHistory;
        executionContext.SkipResult = skipDecision;
        var usedHistoryResult = historicalResult with { ModuleStatus = Status.UsedHistory };
        logger.LogDebug(message);
        return usedHistoryResult;
    }

    private static ModuleResult<T> UseCachedResult<T>(
        ModuleExecutionContext<T> executionContext,
        ModuleResult<T> cachedResult,
        IModuleLogger logger)
    {
        executionContext.Status = Status.CachedResult;
        var result = cachedResult with { ModuleStatus = Status.CachedResult };
        logger.LogDebug("Using cached module result");
        return result;
    }

    private async Task<T> ExecuteWithPolicies<T>(
        Module<T> module,
        ModuleConfiguration config,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext)
    {
        var timeout = GetTimeout(config);
        LogTimeoutConfiguration(config, timeout, moduleContext.Logger);

        var cancellationToken = executionContext.ModuleCancellationTokenSource.Token;

        // Get retry policy if applicable
        var retryPolicy = GetRetryPolicy<T>(config, moduleContext);
        using var retryCancellationTokenSource = retryPolicy is null
            ? null
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var policyExecutionState = new PolicyExecutionState();

        // Keep timeout enforcement inside the retry policy so each attempt gets a fresh budget
        // and policy-owned backoff delays are not mistaken for unresponsive module execution.
        Task<T> ExecuteModuleAttempt(CancellationToken ct) => ExecuteModuleAttemptAsync(
            module,
            executionContext,
            moduleContext,
            timeout,
            retryCancellationTokenSource,
            policyExecutionState,
            ct);

        T result;
        try
        {
            result = retryPolicy != null
                ? await retryPolicy.ExecuteAsync(
                    ExecuteModuleAttempt,
                    retryCancellationTokenSource!.Token).ConfigureAwait(false)
                : await ExecuteModuleAttempt(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (policyExecutionState.AbandonedAttemptTimeout is not null
                                                 && !cancellationToken.IsCancellationRequested)
        {
            return ThrowPreservingStack<T>(policyExecutionState.AbandonedAttemptTimeout);
        }
        finally
        {
            ModuleActivityTracing.RecordModuleRetries(
                executionContext.ModuleType,
                policyExecutionState.RetryCount);
        }

        if (policyExecutionState.AbandonedAttemptTimeout is { } abandonedAttemptTimeout)
        {
            return ThrowPreservingStack<T>(abandonedAttemptTimeout);
        }

        return result;
    }

    private static void LogTimeoutConfiguration(
        ModuleConfiguration config,
        TimeSpan timeout,
        IModuleLogger logger)
    {
        if (config.Timeout is not null)
        {
            return;
        }

        if (timeout == TimeSpan.Zero)
        {
            logger.LogTrace("No module timeout configured. The pipeline default timeout is disabled");
            return;
        }

        logger.LogTrace("No module timeout configured. Using pipeline default timeout {Timeout}", timeout);
    }

    private static async Task<T> ExecuteModuleAttemptAsync<T>(
        Module<T> module,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        TimeSpan timeout,
        CancellationTokenSource? retryCancellationTokenSource,
        PolicyExecutionState policyExecutionState,
        CancellationToken cancellationToken)
    {
        if (policyExecutionState.AbandonedAttemptTimeout is { } abandonedAttemptTimeout)
        {
            return ThrowPreservingStack<T>(abandonedAttemptTimeout);
        }

        policyExecutionState.RecordAttempt();

        var timeoutResult = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            attemptToken => module.ExecuteAsync(moduleContext, attemptToken),
            timeout == TimeSpan.Zero ? null : timeout,
            cancellationToken,
            $"Module {executionContext.ModuleType.Name} timed out after {timeout}").ConfigureAwait(false);

        if (!timeoutResult.TimedOut)
        {
            return timeoutResult.Value!;
        }

        var timeoutException = new ModuleTimeoutException(
            executionContext.ModuleType,
            timeout,
            timeoutResult.ElapsedTime,
            timeoutResult.WasCancellationTokenRespected);

        if (!timeoutResult.WasCancellationTokenRespected)
        {
            policyExecutionState.AbandonedAttemptTimeout = timeoutException;
            // Let wrapped policies observe the failure, but cancel their retry delay because
            // re-entering this module while the abandoned attempt is active is unsafe.
            retryCancellationTokenSource?.Cancel();
        }

        throw timeoutException;
    }

    private static T ThrowPreservingStack<T>(Exception exception)
    {
        System.Runtime.ExceptionServices.ExceptionDispatchInfo
            .Capture(exception)
            .Throw();
        throw new System.Diagnostics.UnreachableException();
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
        await SaveToResultRepository(module, result, moduleContext).ConfigureAwait(false);
        await SaveToModuleCache(module, result, moduleContext, cancellationToken).ConfigureAwait(false);
    }

    private async Task SaveToResultRepository<T>(
        Module<T> module,
        ModuleResult<T> result,
        IModuleContext moduleContext)
    {
        if (!_resultRepository.IsEnabled)
        {
            return;
        }

        try
        {
            await _resultRepository.SaveResultAsync(module, result, moduleContext).ConfigureAwait(false);
        }
        catch (Exception e) when (e is not (OutOfMemoryException or StackOverflowException))
        {
            moduleContext.Logger.LogError(e, "Error saving module result to repository");
        }
    }

    private async Task SaveToModuleCache<T>(
        Module<T> module,
        ModuleResult<T> result,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        if (_pipelineOptions.Value.DisableModuleCache
            || !((IModule) module).Configuration.CacheEnabled
            || _cacheResultRepository is null)
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

    private async Task<ModuleResult<T>> InvokeAfterExecuteAsync<T>(
        Module<T> module,
        IModuleContext moduleContext,
        ModuleResult<T> moduleResult,
        CancellationToken cancellationToken)
    {
        var previousProvisionalResult = module.SetProvisionalResult(moduleResult);
        try
        {
            return await _directHookInvoker
                       .InvokeAfterExecuteAsync(module, moduleContext, moduleResult, cancellationToken)
                       .ConfigureAwait(false)
                   ?? moduleResult;
        }
        catch (Exception afterHookException)
        {
            moduleContext.Logger.LogError(afterHookException, "Error in OnAfterExecuteAsync hook");
            return moduleResult;
        }
        finally
        {
            module.RestoreProvisionalResult(previousProvisionalResult);
        }
    }

    private async Task<ModuleResult<T>> HandleException<T>(
        Module<T> module,
        ModuleConfiguration config,
        ModuleExecutionContext<T> executionContext,
        IModuleContext moduleContext,
        Exception exception,
        IModuleLogger logger,
        Action<ModuleResult<T>> preserveResult,
        bool completeModule = true)
    {
        logger.LogError(exception, "Module failed after {Duration}", executionContext.Duration.ToDisplayString());

        executionContext.Exception = exception;

        executionContext.Status = ClassifyException(config, exception);

        // Use the enhanced exception type for detailed timeout logging.
        if (exception is ModuleTimeoutException timeoutException)
        {
            if (!timeoutException.WasCancellationTokenRespected)
            {
                logger.LogWarning(
                    "Module {ModuleName} did not complete within the cancellation grace period; timeout enforcement stopped waiting after {ElapsedTime}",
                    executionContext.ModuleType.Name,
                    timeoutException.ElapsedTime.ToDisplayString());
            }
        }
        // Check if we should ignore failures
        if (config.IgnoreFailuresCondition != null
            && (executionContext.Status != Status.PipelineTerminated
                || exception is ModuleTimeoutException))
        {
            if (await config.IgnoreFailuresCondition(moduleContext, exception).ConfigureAwait(false))
            {
                logger.LogDebug("Ignoring failures in this module and continuing...");
                executionContext.Status = Status.IgnoredFailure;

                var ignoredResult = ModuleResult<T>.CreateFailure(exception, executionContext);
                preserveResult(ignoredResult);
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

        if (executionContext.Status == Status.PipelineTerminated)
        {
            logger.LogInformation("Pipeline has been canceled");

            var cancelledResult = ModuleResult<T>.CreateFailure(exception, executionContext);
            preserveResult(cancelledResult);
            executionContext.SetTypedResult(cancelledResult);
            return cancelledResult;
        }

        // Create a failed result before cancelling and throwing
        ModuleResult<T> failedResult = ModuleResult<T>.CreateFailure(exception, executionContext);
        preserveResult(failedResult);
        executionContext.SetTypedResult(failedResult);
        if (completeModule)
        {
            module.CompletionSource.TrySetResult(failedResult);
        }

        // Cancel the pipeline and propagate
        CancelPipelineAndThrow(executionContext, moduleContext, exception, logger);

        // This won't be reached, but compiler needs it
        throw exception;
    }

    private Status ClassifyException(
        ModuleConfiguration config,
        Exception exception)
    {
        if (!config.AlwaysRun
            && IsPipelineCancelled(exception))
        {
            return Status.PipelineTerminated;
        }

        return exception is ModuleTimeoutException
            ? Status.TimedOut
            : Status.Failed;
    }

    private bool IsPipelineCancelled(Exception exception)
    {
        return exception is OperationCanceledException or ModuleTimeoutException
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

    private sealed class ModuleExecutionFinalizer<T>(
        Module<T> module,
        ModuleExecutionContext executionContext,
        Func<ModuleResult<T>, CancellationToken, Task>? finalizeExecutionAsync,
        bool completeModule)
    {
        public bool WasInvoked { get; private set; }

        public async Task FinalizeAsync(ModuleResult<T> result)
        {
            if (finalizeExecutionAsync is null)
            {
                return;
            }

            WasInvoked = true;
            var previousProvisionalResult = module.SetProvisionalResult(result);
            try
            {
                await finalizeExecutionAsync(
                        result,
                        executionContext.ModuleCancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
            finally
            {
                module.RestoreProvisionalResult(previousProvisionalResult);
            }
        }

        public void Complete(ModuleResult<T> result)
        {
            if (completeModule)
            {
                module.CompletionSource.TrySetResult(result);
            }
        }
    }

    private sealed class PolicyExecutionState
    {
        private int _moduleAttemptCount;

        public ModuleTimeoutException? AbandonedAttemptTimeout { get; set; }

        public int RetryCount => Math.Max(0, Volatile.Read(ref _moduleAttemptCount) - 1);

        public void RecordAttempt() => Interlocked.Increment(ref _moduleAttemptCount);
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
            Status.DependencyFailed => LogLevel.Error,
            Status.UsedHistory => LogLevel.Information,
            Status.CachedResult => LogLevel.Information,
            _ => LogLevel.Error,
        };

        logger.Log(logLevel, message);
    }
}
