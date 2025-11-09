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

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(30);

    public ModuleBehaviorExecutor(
        ILogger<ModuleBehaviorExecutor> logger,
        IOptions<PipelineOptions> pipelineOptions)
    {
        _logger = logger;
        _pipelineOptions = pipelineOptions;
    }

    public async Task<T?> ExecuteAsync<T>(IModule<T> module, IPipelineContext context, CancellationToken cancellationToken)
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

            var result = await ExecuteWithRetryAndTimeout(module, context, cancellationToken);

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

                    mod5.Status = exception is ModuleTimeoutException ? Status.TimedOut : Status.Failed;
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
        CancellationToken cancellationToken)
    {
        IAsyncPolicy retryPolicy = GetRetryPolicy(module);
        TimeSpan timeout = GetTimeout(module);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        if (timeout != TimeSpan.Zero)
        {
            timeoutCts.CancelAfter(timeout);
        }

        var executeTask = retryPolicy.ExecuteAsync(async () =>
        {
            if (timeout != TimeSpan.Zero && timeoutCts.IsCancellationRequested)
            {
                throw new ModuleTimeoutException(module);
            }

            timeoutCts.Token.ThrowIfCancellationRequested();

            try
            {
                return await module.ExecuteAsync(context, timeoutCts.Token);
            }
            catch (OperationCanceledException) when (timeout != TimeSpan.Zero && timeoutCts.IsCancellationRequested)
            {
                throw new ModuleTimeoutException(module);
            }
        });

        if (timeout != TimeSpan.Zero)
        {
            var timeoutTask = CreateTimeoutTask(timeout, timeoutCts.Token, module);

            _ = executeTask.ContinueWith(
                t => _ = t.Exception,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            await await Task.WhenAny(timeoutTask, executeTask);
        }
        else
        {
            await executeTask;
        }

        timeoutCts.Token.ThrowIfCancellationRequested();

        return await executeTask;
    }

    private async Task CreateTimeoutTask<T>(TimeSpan timeout, CancellationToken cancellationToken, IModule<T> module)
    {
        try
        {
            await Task.Delay(timeout, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        if (module is Module<T> mod && mod.Status != Status.Successful)
        {
            throw new ModuleTimeoutException(module);
        }
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
                .Handle<Exception>()
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
