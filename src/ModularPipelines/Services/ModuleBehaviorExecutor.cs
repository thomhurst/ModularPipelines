using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using Polly;

namespace ModularPipelines.Services;

/// <summary>
/// Orchestrates module execution with all configured behaviors.
/// Replaces the StartInternal/ExecuteInternal logic previously embedded in Module&lt;T&gt;.
/// </summary>
public class ModuleBehaviorExecutor : IModuleBehaviorExecutor
{
    private readonly ILogger<ModuleBehaviorExecutor> _logger;

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(30);

    public ModuleBehaviorExecutor(ILogger<ModuleBehaviorExecutor> logger)
    {
        _logger = logger;
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
        if (module is not IModuleSkipLogic skipLogic)
        {
            return SkipDecision.DoNotSkip;
        }

        return await skipLogic.ShouldSkipAsync(context);
    }

    private async Task<bool> ShouldSkipModule(IModule module, IPipelineContext context)
    {
        var skipDecision = await GetSkipDecision(module, context);
        return skipDecision.ShouldSkip;
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
