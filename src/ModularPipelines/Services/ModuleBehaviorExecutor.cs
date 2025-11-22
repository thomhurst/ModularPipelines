using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.Options;
using Polly;

namespace ModularPipelines.Services;

/// <summary>
/// Orchestrates module execution with all configured behaviors.
/// Replaces the StartInternal/ExecuteInternal logic previously embedded in Module&lt;T&gt;.
/// </summary>
public class ModuleBehaviorExecutor : IModuleBehaviorExecutor
{
    private readonly ILogger<ModuleBehaviorExecutor> _logger;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IModuleStateResolver _moduleStateResolver;

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(30);

    public ModuleBehaviorExecutor(
        ILogger<ModuleBehaviorExecutor> logger,
        IOptions<PipelineOptions> pipelineOptions,
        IModuleStateResolver moduleStateResolver)
    {
        _logger = logger;
        _pipelineOptions = pipelineOptions;
        _moduleStateResolver = moduleStateResolver;
    }

    public async Task<T?> ExecuteAsync<T>(IModule<T> module, IPipelineContext context, CancellationToken cancellationToken, CancellationToken pipelineCancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var skipDecision = await GetSkipDecision(module, context);
            if (skipDecision.ShouldSkip)
            {
                _logger.LogInformation("Module {ModuleName} was skipped: {Reason}", module.ModuleType.Name, skipDecision.Reason);

                if (module is Module<T> mod)
                {
                    mod.Status = Status.Skipped;
                    mod.SkipDecision = skipDecision;
                }

                return default;
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (module is IModuleLifecycle lifecycle)
            {
                await lifecycle.OnBeforeExecuteAsync(context);
            }

            if (module is Module<T> mod1)
            {
                mod1.Status = Status.Processing;
                mod1.StartTime = DateTimeOffset.UtcNow;

                _logger.LogDebug("Module {ModuleName} execution started at {StartTime}",
                    module.ModuleType.Name,
                    mod1.StartTime);
            }

            var result = await ExecuteWithRetryAndTimeout(module, context, cancellationToken, pipelineCancellationToken);

            if (module is Module<T> mod2)
            {
                mod2.EndTime = DateTimeOffset.UtcNow;
                mod2.Status = Status.Successful;
            }

            // Cache the result in the module for later retrieval
            if (module is Module<T> moduleInstance)
            {
                moduleInstance.Value = result;

                _logger.LogDebug("Module {ModuleName} succeeded after {Duration}",
                    module.ModuleType.Name,
                    moduleInstance.Duration);
            }

            LogResult(context, result);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogDebug("Module {ModuleName} caught exception of type {ExceptionType} with message: {ExceptionMessage}",
                module.ModuleType.Name, exception.GetType().Name, exception.Message);

            if (module is Module<T> mod3)
            {
                mod3.EndTime = DateTimeOffset.UtcNow;
                mod3.Exception = exception;
            }

            bool ignoreFailure = false;
            if (module is IModuleErrorHandling errorHandling)
            {
                ignoreFailure = await errorHandling.ShouldIgnoreFailuresAsync(context, exception);
            }

            if (ignoreFailure)
            {
                _logger.LogWarning(exception, "Module {ModuleName} failed but failure is ignored", module.ModuleType.Name);

                if (module is Module<T> mod4)
                {
                    mod4.Status = Status.IgnoredFailure;
                }

                return default;
            }
            else
            {
                if (module is Module<T> mod5)
                {
                    _logger.LogError(exception, "Module {ModuleName} failed after {Duration}",
                        module.ModuleType.Name,
                        mod5.Duration);

                    // Use ModuleStateResolver to determine the correct status
                    // Pass both the module-level token (includes timeouts and worker pool cancellation)
                    // and the pipeline-level token (user/engine cancellation)
                    mod5.Status = _moduleStateResolver.ResolveFailureStatus(module, exception, cancellationToken, pipelineCancellationToken);
                }

                throw;
            }
        }
        finally
        {
            if (module is IModuleLifecycle lifecycle)
            {
                object? result = default;
                Exception? exc = null;

                if (module is Module<T> modFinal)
                {
                    result = modFinal.Status == Status.Successful ? await GetResultSafe(module, context, cancellationToken) : default(object);
                    exc = modFinal.Exception;
                }

                await lifecycle.OnAfterExecuteAsync(context, result, exc);
            }

            stopwatch.Stop();

            if (module is Module<T> modLog)
            {
                _logger.LogInformation("Module {ModuleName} finished with status {Status}",
                    module.ModuleType.Name,
                    modLog.Status);
            }
        }
    }

    private async Task<T?> ExecuteWithRetryAndTimeout<T>(
        IModule<T> module,
        IPipelineContext context,
        CancellationToken cancellationToken,
        CancellationToken pipelineCancellationToken)
    {
        IAsyncPolicy retryPolicy = GetRetryPolicy(module);
        TimeSpan timeout = GetTimeout(module);

        // Link BOTH the module's cancellation token AND the pipeline cancellation token
        // so that modules are cancelled when either the timeout occurs OR the pipeline is cancelled
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, pipelineCancellationToken);

        if (timeout != TimeSpan.Zero)
        {
            timeoutCts.CancelAfter(timeout);
        }

        // Execute with retry policy, but don't retry on OperationCanceledException
        var executeTask = retryPolicy.ExecuteAsync(async () =>
        {
            if (timeout != TimeSpan.Zero && timeoutCts.IsCancellationRequested)
            {
                // Check if pipeline was cancelled first - don't treat it as a timeout
                if (pipelineCancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException(pipelineCancellationToken);
                }

                throw new ModuleTimeoutException(module);
            }

            timeoutCts.Token.ThrowIfCancellationRequested();

            try
            {
                return await module.ExecuteAsync(context, timeoutCts.Token);
            }
            catch (OperationCanceledException) when (timeout != TimeSpan.Zero && timeoutCts.IsCancellationRequested)
            {
                // If the pipeline was cancelled, rethrow the OperationCanceledException
                // The ModuleStateResolver will determine it's a PipelineTerminated status
                if (pipelineCancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                // Otherwise, it's a genuine timeout
                throw new ModuleTimeoutException(module);
            }
        });

        if (timeout != TimeSpan.Zero)
        {
            // Avoid unobserved task exceptions if the timeout occurs before executeTask is awaited.
            _ = executeTask.ContinueWith(
                t => _ = t.Exception,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            _logger.LogDebug("Module {ModuleName}: Waiting for timeout or completion (timeout={Timeout}s)",
                module.ModuleType.Name, timeout.TotalSeconds);

            // This delay acts as a safety net. It is NOT cancelled by any token.
            // It ensures that if the module ignores cancellation, we don't hang forever waiting for executeTask.
            // We use CancellationToken.None so the delay always runs for the full timeout duration.
            var timeoutDelayTask = Task.Delay(timeout, CancellationToken.None);

            var completedTask = await Task.WhenAny(executeTask, timeoutDelayTask);

            _logger.LogDebug("Module {ModuleName}: First task completed. TimeoutDelayTask={IsTimeout}, ExecuteTask={IsExecute}",
                module.ModuleType.Name, completedTask == timeoutDelayTask, completedTask == executeTask);

            if (completedTask == timeoutDelayTask)
            {
                // The safety net delay finished. This means the module execution has timed out
                // and is not responding to the cancellation signal sent by `timeoutCts.CancelAfter`.
                // We must throw to prevent a hang.

                // Prioritize reporting pipeline cancellation if that was the cause.
                pipelineCancellationToken.ThrowIfCancellationRequested();

                // Otherwise, it's a hard timeout because the module was unresponsive.
                throw new ModuleTimeoutException(module);
            }
        }

        // Check if pipeline was cancelled - throw with pipeline token to preserve cancellation source
        _logger.LogDebug("Module {ModuleName}: Checking pipeline cancellation. Requested={Cancelled}",
            module.ModuleType.Name, pipelineCancellationToken.IsCancellationRequested);

        if (pipelineCancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(pipelineCancellationToken);
        }

        _logger.LogDebug("Module {ModuleName}: Checking timeout cancellation. Requested={Cancelled}",
            module.ModuleType.Name, timeoutCts.Token.IsCancellationRequested);

        // This will throw ModuleTimeoutException if timeout occurred and was handled gracefully by the module.
        timeoutCts.Token.ThrowIfCancellationRequested();

        _logger.LogDebug("Module {ModuleName}: Re-awaiting executeTask to get result", module.ModuleType.Name);

        return await executeTask;
    }

    private async Task<SkipDecision> GetSkipDecision(IModule module, IPipelineContext context)
    {
        var categoryDecision = CheckCategoryFilters(module);
        if (categoryDecision.ShouldSkip)
        {
            return categoryDecision;
        }

        var mandatoryDecision = await CheckMandatoryRunConditions(module, context);
        if (mandatoryDecision.ShouldSkip)
        {
            return mandatoryDecision;
        }

        var runConditionDecision = await CheckRunConditions(module, context);
        if (runConditionDecision.ShouldSkip)
        {
            return runConditionDecision;
        }

        if (module is IModuleSkipLogic skipLogic)
        {
            return await skipLogic.ShouldSkipAsync(context);
        }

        return SkipDecision.DoNotSkip;
    }

    private async Task<bool> ShouldSkipModule(IModule module, IPipelineContext context)
    {
        var skipDecision = await GetSkipDecision(module, context);
        return skipDecision.ShouldSkip;
    }

    private SkipDecision CheckCategoryFilters(IModule module)
    {
        var categoryAttr = module.ModuleType.GetCustomAttribute<ModuleCategoryAttribute>();
        if (categoryAttr == null)
        {
            return SkipDecision.DoNotSkip;
        }

        var options = _pipelineOptions.Value;

        if (options.IgnoreCategories?.Contains(categoryAttr.Category) == true)
        {
            return $"Module category '{categoryAttr.Category}' is in the ignore list";
        }

        if (options.RunOnlyCategories?.Any() == true && !options.RunOnlyCategories.Contains(categoryAttr.Category))
        {
            return $"Module category '{categoryAttr.Category}' is not in the runnable categories list";
        }

        return SkipDecision.DoNotSkip;
    }

    private async Task<SkipDecision> CheckMandatoryRunConditions(IModule module, IPipelineContext context)
    {
        var mandatoryAttributes = module.ModuleType
            .GetCustomAttributes<MandatoryRunConditionAttribute>(inherit: true)
            .ToList();

        if (!mandatoryAttributes.Any())
        {
            return SkipDecision.DoNotSkip;
        }

        var evaluationTasks = mandatoryAttributes
            .Select(async attr => new
            {
                Attribute = attr,
                Result = await attr.Condition(context)
            })
            .ToList();

        var evaluations = await Task.WhenAll(evaluationTasks);

        var failedCondition = evaluations.FirstOrDefault(e => !e.Result);
        if (failedCondition != null)
        {
            var attributeName = failedCondition.Attribute.GetType().Name.Replace("Attribute", "");
            return $"Mandatory run condition failed: {attributeName}";
        }

        return SkipDecision.DoNotSkip;
    }

    private async Task<SkipDecision> CheckRunConditions(IModule module, IPipelineContext context)
    {
        var regularAttributes = module.ModuleType
            .GetCustomAttributes<RunConditionAttribute>(inherit: true)
            .Where(attr => attr is not MandatoryRunConditionAttribute)
            .ToList();

        if (!regularAttributes.Any())
        {
            return SkipDecision.DoNotSkip;
        }

        var evaluationTasks = regularAttributes
            .Select(async attr => new
            {
                Attribute = attr,
                Result = await attr.Condition(context)
            })
            .ToList();

        var evaluations = await Task.WhenAll(evaluationTasks);

        if (evaluations.Any(e => e.Result))
        {
            return SkipDecision.DoNotSkip;
        }

        var failedConditionNames = evaluations
            .Select(e => e.Attribute.GetType().Name.Replace("Attribute", ""))
            .ToList();

        var conditionsList = string.Join(", ", failedConditionNames);
        return $"No run conditions were met. Failed conditions: {conditionsList}";
    }

    private IAsyncPolicy GetRetryPolicy(IModule module)
    {
        if (module is IModuleRetryPolicy retryBehavior)
        {
            return retryBehavior.GetRetryPolicy();
        }

        var retryAttr = module.ModuleType.GetCustomAttribute<Attributes.RetryAttribute>();
        if (retryAttr != null && retryAttr.Count > 0)
        {
            return Policy
                .Handle<Exception>(ex => ex is not OperationCanceledException)
                .WaitAndRetryAsync(
                    retryCount: retryAttr.Count,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(retryAttr.BackoffSeconds * Math.Pow(attempt, 2)));
        }

        return Policy.NoOpAsync();
    }

    private TimeSpan GetTimeout(IModule module)
    {
        if (module is IModuleTimeout timeoutBehavior)
        {
            return timeoutBehavior.GetTimeout();
        }

        var timeoutAttr = module.ModuleType.GetCustomAttribute<Attributes.TimeoutAttribute>();
        if (timeoutAttr != null)
        {
            return timeoutAttr.Timeout;
        }

        return DefaultTimeout;
    }

    private void LogResult<T>(IPipelineContext context, T? result)
    {
        if (!context.Logger.IsEnabled(LogLevel.Debug))
        {
            return;
        }

        try
        {
            context.Logger.LogDebug("Module returned {Type}:", result?.GetType().Name ?? typeof(T).Name);

            if (result is null)
            {
                context.Logger.LogDebug("null");
                return;
            }

            if (typeof(T).IsPrimitive || result is string)
            {
                context.Logger.LogDebug("{Value}", result);
                return;
            }

            context.Logger.LogDebug("{TypeName}", result.GetType().Name);
        }
        catch (Exception ex)
        {
            context.Logger.LogWarning(ex, "Failed to log result");
        }
    }

    private async Task<object?> GetResultSafe<T>(IModule<T> module, IPipelineContext context, CancellationToken cancellationToken)
    {
        try
        {
            return await module.ExecuteAsync(context, cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}
