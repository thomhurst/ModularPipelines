using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Executors.ModuleHandlers;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
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
    private readonly object _startCheckLock = new();

    internal override IHistoryHandler<T> HistoryHandler { get; }

    internal override IWaitHandler WaitHandler { get; }

    internal override ICancellationHandler CancellationHandler { get; }

    internal override ISkipHandler SkipHandler { get; }

    internal override IHookHandler HookHandler { get; }

    internal override IStatusHandler StatusHandler { get; }

    internal override IErrorHandler ErrorHandler { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="Module{T}"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [JsonConstructor]
    protected Module()
    {
        WaitHandler = new WaitHandler<T>(this);
        CancellationHandler = new CancellationHandler<T>(this);
        SkipHandler = new SkipHandler<T>(this);
        HistoryHandler = new HistoryHandler<T>(this);
        HookHandler = new HookHandler<T>(this);
        StatusHandler = new StatusHandler<T>(this);
        ErrorHandler = new ErrorHandler<T>(this);

        foreach (var customAttribute in GetType().GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            AddDependency(customAttribute);
        }
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

    internal override Task ExecutionTask
    {
        [StackTraceHidden]
        get
        {
            lock (_startCheckLock)
            {
                if (!IsStarted)
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    StartInternal();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }

                IsStarted = true;

                return ModuleResultTaskCompletionSource.Task;
            }
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

    private void AddDependency(DependsOnAttribute dependsOnAttribute)
    {
        var type = dependsOnAttribute.Type;

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
            Context.Logger.LogDebug("This module depends on {Module}", dependsOnAttribute.Type.Name);
        };

        DependentModules.Add(dependsOnAttribute);
    }

    [StackTraceHidden]
    private async Task StartInternal()
    {
        if (IsStarted || ModuleResultTaskCompletionSource.Task.IsCompleted)
        {
            return;
        }

        try
        {
            if (await WaitHandler.WaitForModuleDependencies() == WaitResult.Abort)
            {
                return;
            }

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

    private void LogResult(T? executeResult)
    {
        Context.Logger.LogDebug("Module returned {Type}:", executeResult?.GetType().Name ?? typeof(T).Name);
        
        if (executeResult is null)
        {
            Context.Logger.LogDebug("null");
            return;
        }

        if (executeResult is IEnumerable enumerable)
        {
            foreach (var o in enumerable.Cast<object>())
            {
                Context.Logger.LogDebug("{Json}", JsonSerializer.Serialize(o));
            }
            
            return;
        }

        if (typeof(T).IsPrimitive || executeResult is string)
        {
            Context.Logger.LogDebug("{Value}", executeResult);
            return;
        }
        
        Context.Logger.LogDebug("{Json}", JsonSerializer.Serialize(executeResult));
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
            return await executeAsyncTask;
        }
        
        ModuleCancellationTokenSource.CancelAfter(Timeout);

        var timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ModuleCancellationTokenSource.Token);
        _ = executeAsyncTask.ContinueWith(async t =>
        {
            await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None);
            timeoutCancellationTokenSource.Cancel();
            timeoutCancellationTokenSource.Dispose();
        }, CancellationToken.None);
        
        var timeoutExceptionTask = Task.Delay(Timeout, timeoutCancellationTokenSource.Token)
            .ContinueWith(t =>
            {
                if (ModuleRunType == ModuleRunType.OnSuccessfulDependencies)
                {
                    Context.EngineCancellationToken.Token.ThrowIfCancellationRequested();
                }

                throw new ModuleTimeoutException(this);
            }, CancellationToken.None);

        // Will throw a timeout exception if configured and timeout is reached
        await await Task.WhenAny(timeoutExceptionTask, executeAsyncTask);

        return await executeAsyncTask;
    }

    private void SetEndTime()
    {
        _stopwatch.Stop();
        EndTime = DateTimeOffset.UtcNow;
        Duration = _stopwatch.Elapsed;
    }
}