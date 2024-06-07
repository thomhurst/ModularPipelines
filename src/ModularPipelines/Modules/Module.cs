using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Executors.ModuleHandlers;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Serialization;
using Polly;
using Polly.Retry;

namespace ModularPipelines.Modules;

public abstract class Module : Module<IDictionary<string, object>>;

/// <summary>
/// An independent module used to perform an action, and optionally return some data, which can be used within other modules. This is the base class from which all custom modules should inherit.
/// </summary>
/// <typeparam name="T">The data to return which can be used within other modules, which is returned from its ExecuteAsync method..</typeparam>
public abstract partial class Module<T> : ModuleBase<T>
{
    private readonly Stopwatch _stopwatch = new();

    internal override IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetModuleDependencies()
    {
        foreach (var customAttribute in GetType().GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            yield return AddDependency(customAttribute.Type, customAttribute.IgnoreIfNotRegistered);
        }
        
        foreach (var customAttribute in GetType().GetCustomAttributesIncludingBaseInterfaces<DependsOnAllModulesInheritingFromAttribute>())
        {
            var types = Context.ServiceProvider.GetServices<ModuleBase>()
                .Where(x => x.GetType().IsOrInheritsFrom(customAttribute.Type));
            
            foreach (var moduleBase in types)
            {
                yield return AddDependency(moduleBase.GetType(), false);   
            }
        }
    }

    internal override IHistoryHandler<T> HistoryHandler { get; }
    
    internal override ICancellationHandler CancellationHandler { get; }

    internal override ISkipHandler SkipHandler { get; }

    internal override IHookHandler HookHandler { get; }

    internal override IStatusHandler StatusHandler { get; }

    internal override IErrorHandler ErrorHandler { get; }

    public async Task<ModuleResult<T>> GetResult() => await this;

    /// <summary>
    /// Initialises a new instance of the <see cref="Module{T}"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [JsonConstructor]
    protected Module()
    {
        CancellationHandler = new CancellationHandler<T>(this);
        SkipHandler = new SkipHandler<T>(this);
        HistoryHandler = new HistoryHandler<T>(this);
        HookHandler = new HookHandler<T>(this);
        StatusHandler = new StatusHandler<T>(this);
        ErrorHandler = new ErrorHandler<T>(this);
    }

    internal override ModuleBase Initialize(IPipelineContext context)
    {
        context.InitializeLogger(GetType());
        Context = context;
        return this;
    }

    [JsonInclude]
    internal ModuleResult<T> Result
    {
        get
        {
            return _result;
        }

        set
        {
            _result = value;
            SetResult(value);
        }
    }

    internal override Task ExecutionTask => ModuleResultTaskCompletionSource.Task;

    internal override async Task<IModuleResult> GetModuleResult() => await this;
    
    internal override async Task StartInternal()
    {
        if (IsStarted || ModuleResultTaskCompletionSource.Task.IsCompleted)
        {
            return;
        }

        IsStarted = true;

        try
        {
            CancellationHandler.SetupCancellation();

            if (await SkipHandler.HandleSkipped())
            {
                return;
            }

            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            await HookHandler.OnBeforeExecute(Context);

            StartTask.Start(TaskScheduler.Default);

            Status = Status.Processing;
            StartTime = DateTimeOffset.UtcNow;

            _stopwatch.Start();

            var executeResult = await ExecuteInternal();

            SetEndTime();

            LogResult(executeResult);

            Status = Status.Successful;

            var moduleResult = new ModuleResult<T>(executeResult, this);

            await HistoryHandler.SaveResult(moduleResult);

            Context.Logger.LogDebug("Module Succeeded after {Duration}", Duration);
        }
        catch (Exception exception)
        {
            SetEndTime();
            await ErrorHandler.Handle(exception);
        }
        finally
        {
            await HookHandler.OnAfterExecute(Context);

            StatusHandler.LogModuleStatus();
        }
    }
    
    /// <summary>
    /// Gets the Module of type <see cref="TModule">{TModule}</see>.
    /// </summary>
    /// <typeparam name="TModule">The type of module to get.</typeparam>
    /// <returns>{TModule}.</returns>
    /// <exception cref="ModuleNotRegisteredException">Thrown if the module has not been registered.</exception>
    /// <exception cref="ModuleReferencingSelfException">Thrown if the module tries to get itself.</exception>
    protected TModule GetModule<TModule>()
        where TModule : ModuleBase
    {
        var module = GetModuleIfRegistered<TModule>();

        if (module is null)
        {
            throw new ModuleNotRegisteredException(
                $"The module {typeof(TModule)} has not been registered", null);
        }

        return module;
    }

    /// <summary>
    /// Gets the Module of type <see cref="TModule">{TModule}</see>, or null if it is not registered.
    /// </summary>
    /// <typeparam name="TModule">The type of module to get.</typeparam>
    /// <returns>{TModule}.</returns>
    /// <exception cref="ModuleReferencingSelfException">Thrown if the module tries to get itself.</exception>
    protected TModule? GetModuleIfRegistered<TModule>()
        where TModule : ModuleBase
    {
        if (typeof(TModule) == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot get itself");
        }

        return Context.GetModule<TModule>();
    }

    /// <summary>
    /// Creates a generic Retry policy that'll catch any exception and retry.
    /// </summary>
    /// <param name="count">The amount of times to retry.</param>
    /// <returns>{T}.</returns>
    protected AsyncRetryPolicy<T?> CreateRetryPolicy(int count) =>
        Policy<T?>.Handle<Exception>()
            .WaitAndRetryAsync(count, i => TimeSpan.FromMilliseconds(i * i * 100));

    private (Type Type, bool IgnoreIfNotRegistered) AddDependency(Type type, bool ignoreIfNotRegistered)
    {
        if (type == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot depend on itself");
        }

        if (!type.IsAssignableTo(typeof(ModuleBase)))
        {
            throw new Exception($"{type.FullName} must be a module to add as a dependency");
        }

        OnInitialised += (_, _) =>
        {
            Context.Logger.LogDebug("This module depends on {Module}", type.Name);
        };

        return (type, ignoreIfNotRegistered);
    }

    private void LogResult(T? executeResult)
    {
        if (!Context.Logger.IsEnabled(LogLevel.Debug))
        {
            return;
        }

        try
        {
            Context.Logger.LogDebug("Module returned {Type}:", executeResult?.GetType().GetRealTypeName() ?? typeof(T).GetRealTypeName());

            if (executeResult is null)
            {
                Context.Logger.LogDebug("null");
                return;
            }

            if (typeof(T).IsPrimitive || executeResult is string)
            {
                Context.Logger.LogDebug("{Value}", executeResult);
                return;
            }

            if (executeResult is IEnumerable enumerable)
            {
                foreach (var o in enumerable.Cast<object>())
                {
                    Context.Logger.LogDebug("{JsonUtils}", JsonSerializer.Serialize(o, ModularPipelinesJsonSerializerSettings.Default));
                }

                return;
            }

            Context.Logger.LogDebug("{JsonUtils}", JsonSerializer.Serialize(executeResult));
        }
        catch
        {
            Context.Logger.LogDebug("{Value}", executeResult);
        }
    }

    private void SetResult(ModuleResult<T> result)
    {
        result.Module ??= this;

        Duration = result.ModuleDuration;
        StartTime = result.ModuleStart;
        EndTime = result.ModuleEnd;

        SkipResult = result.SkipDecision;

        Exception = result.Exception;

        ModuleResultTaskCompletionSource.TrySetResult(result);
    }

    private ModuleResult<T> _result = null!;

    private async Task<T?> ExecuteInternal()
    {
        var isRetry = false;
        var executeAsyncTask = RetryPolicy.ExecuteAsync(() =>
        {
            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            if (isRetry)
            {
                Context.Logger.LogWarning("An error occurred. Retrying...");
            }

            isRetry = true;

            return ExecuteAsync(Context, ModuleCancellationTokenSource.Token);
        });

        if (Timeout == TimeSpan.Zero)
        {
            await await Task.WhenAny(executeAsyncTask, ThrowQuicklyOnFailure(executeAsyncTask, null));
            return await executeAsyncTask;
        }

        ModuleCancellationTokenSource.CancelAfter(Timeout);

        using var timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ModuleCancellationTokenSource.Token);

        var timeoutExceptionTask = Task.Delay(Timeout, timeoutCancellationTokenSource.Token)
            .ContinueWith(t =>
            {
                if (executeAsyncTask.IsCompleted)
                {
                    return;
                }
                
                if (ModuleRunType == ModuleRunType.OnSuccessfulDependencies)
                {
                    Context.EngineCancellationToken.Token.ThrowIfCancellationRequested();
                }

                throw new ModuleTimeoutException(this);
            }, CancellationToken.None);

        var finishedTask = await Task.WhenAny(timeoutExceptionTask, executeAsyncTask, ThrowQuicklyOnFailure(executeAsyncTask, timeoutExceptionTask));

#if NET8_0_OR_GREATER
        await timeoutCancellationTokenSource.CancelAsync();
#else
        timeoutCancellationTokenSource.Cancel();
#endif
        
        // Will throw a timeout exception if configured and timeout is reached
        await finishedTask;

        return await executeAsyncTask;
    }

    private async Task ThrowQuicklyOnFailure(IAsyncResult mainExecutionTask, IAsyncResult? timeoutTask)
    {
        while (!mainExecutionTask.IsCompleted)
        {
            if (timeoutTask?.IsCompleted == true)
            {
                throw new ModuleTimeoutException(this);
            }
            
            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();
            
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    private void SetEndTime()
    {
        _stopwatch.Stop();
        EndTime = DateTimeOffset.UtcNow;
        Duration = _stopwatch.Elapsed;
    }
}