using System.Diagnostics;
using System.Text.Json.Serialization;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public abstract class Module : Module<IDictionary<string, object>>
{
}

/// <summary>
/// An independent module used to perform an action, and optionally return some data, which can be used within other modules. This is the base class from which all custom modules should inherit.
/// </summary>
/// <typeparam name="T">The data to return which can be used within other modules, which is returned from its ExecuteAsync method..</typeparam>
public abstract partial class Module<T> : ModuleBase<T>
{
    private readonly Stopwatch _stopwatch = new();

    internal List<DependsOnAttribute> DependentModules { get; } = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="Module{T}"/> class.
    /// </summary>
    [JsonConstructor]
    protected Module()
    {
        foreach (var customAttribute in GetType().GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            AddDependency(customAttribute);
        }
    }

    /// <summary>
    /// Used to start, run, and control the flow of a Module, including handling exceptions and skipping.
    /// </summary>
    /// <exception cref="ModuleFailedException">Thrown if the module has failed and the failure was not ignored.</exception>
    internal override async Task StartAsync()
    {
        try
        {
            try
            {
                await WaitForModuleDependencies();
            }
            catch when (Context.EngineCancellationToken.IsCancellationRequested && ModuleRunType == ModuleRunType.OnSuccessfulDependencies)
            {
                // The Engine has requested a cancellation due to failures - So fail fast and don't repeat exceptions thrown by other modules.
                Context.Logger.LogDebug("The pipeline has been cancelled before this module started");
                
                ModuleResultTaskCompletionSource.TrySetCanceled();
                return;
            }
            
            if (ModuleRunType != ModuleRunType.AlwaysRun)
            {
                Context.EngineCancellationToken.Token.Register(ModuleCancellationTokenSource.Cancel);
            }
            
            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            var skipResult = await ShouldSkip(Context);

            if (skipResult.ShouldSkip)
            {
                await SetSkipped(skipResult);
                return;
            }

            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            await OnBeforeExecute(Context);

            StartTask.Start(TaskScheduler.Default);

            Status = Status.Processing;
            StartTime = DateTimeOffset.UtcNow;

            var timeoutExceptionTask = Task.CompletedTask;

            if (Timeout != TimeSpan.Zero)
            {
                ModuleCancellationTokenSource.CancelAfter(Timeout);
                timeoutExceptionTask = Task.Delay(Timeout + TimeSpan.FromSeconds(30), ModuleCancellationTokenSource.Token);
            }

            _stopwatch.Start();

            var executeResult = await ExecuteInternal(timeoutExceptionTask);

            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;

            Status = Status.Successful;
            EndTime = DateTimeOffset.UtcNow;

            var moduleResult = new ModuleResult<T>(executeResult, this);
            
            await SaveResult(moduleResult);
            
            Context.Logger.LogDebug("Module Succeeded after {Duration}", Duration);
        }
        catch (Exception exception)
        {
            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;

            Context.Logger.LogError(exception, "Module Failed after {Duration}", Duration);

            if (exception is TaskCanceledException or OperationCanceledException
                && ModuleCancellationTokenSource.IsCancellationRequested
                && !Context.EngineCancellationToken.IsCancellationRequested)
            {
                Context.Logger.LogDebug("Module timed out: {ModuleType}", GetType().FullName);

                Status = Status.TimedOut;
            }
            else if (exception is TaskCanceledException or OperationCanceledException
                     && Context.EngineCancellationToken.IsCancellationRequested)
            {
                Status = Status.PipelineTerminated;
                Context.Logger.LogInformation("Pipeline has been canceled");
                return;
            }
            else
            {
                Status = Status.Failed;
            }

            Exception = exception;

            if (await ShouldIgnoreFailures(Context, exception))
            {
                Context.Logger.LogDebug("Ignoring failures in this module and continuing...");
                
                var moduleResult = new ModuleResult<T>(exception, this);

                Status = Status.IgnoredFailure;

                await SaveResult(moduleResult);
            }
            else
            {
                Context.Logger.LogDebug("Module failed. Cancelling the pipeline");
                
                Context.EngineCancellationToken.Cancel();

                // Time for cancellation to register
                await Task.Delay(TimeSpan.FromMilliseconds(200));

                ModuleResultTaskCompletionSource.TrySetException(exception);

                throw new ModuleFailedException(this, exception);
            }
        }
        finally
        {
            ModuleResultTaskCompletionSource.TrySetCanceled();

            await OnAfterExecute(Context);

            LogModuleStatus();
        }
    }

    internal override ModuleBase Initialize(IPipelineContext context)
    {
        context.FetchLogger(GetType());
        Context = context;
        return this;
    }

    internal override async Task SetSkipped(SkipDecision skipDecision)
    {
        Status = Status.Skipped;

        SkipResult = skipDecision;

        if (await UseResultFromHistoryIfSkipped(Context)
            && await SetupModuleFromHistory(skipDecision.Reason))
        {
            return;
        }

        SkippedTask.Start(TaskScheduler.Default);

        ModuleResultTaskCompletionSource.TrySetResult(new SkippedModuleResult<T>(this, skipDecision));

        Context.Logger.LogInformation("{Module} ignored because: {Reason} and no historical results were found", GetType().Name, skipDecision.Reason);
    }

    /// <summary>
    /// Gets the Module of type <see cref="TModule">{TModule}</see>
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

    private async Task<bool> SetupModuleFromHistory(string? skipDecisionReason)
    {
        if (Context.ModuleResultRepository.GetType() == typeof(NoOpModuleResultRepository))
        {
            Context.Logger.LogDebug("No results repository configured to pull historical results from");
            return false;
        }

        var result = await Context.ModuleResultRepository.GetResultAsync<T>(this, Context);

        if (result == null)
        {
            return false;
        }
        
        Context.Logger.LogDebug("Setting up module from history");

        Status = Status.UsedHistory;

        Result = result;

        return true;
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

    [JsonInclude]
    private ModuleResult<T> Result
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

    private async Task WaitForModuleDependencies()
    {
        if (!DependentModules.Any())
        {
            Context.Logger.LogDebug("No dependent modules - Nothing to wait for");
            return;
        }

        try
        {
            await DependentModules
                .ToAsyncProcessorBuilder()
                .ForEachAsync(dependsOnAttribute =>
                {
                    var module = Context.GetModule(dependsOnAttribute.Type);

                    if (dependsOnAttribute.IgnoreIfNotRegistered && module is null)
                    {
                        Context.Logger.LogDebug("{Module} was not registered so not waiting", dependsOnAttribute.Type.Name);
                        return Task.CompletedTask;
                    }

                    if (module is null)
                    {
                        throw new ModuleNotRegisteredException(
                            $"The module {dependsOnAttribute.Type.Name} has not been registered", null);
                    }

                    Context.Logger.LogDebug("Waiting for {Module}", dependsOnAttribute.Type.Name);
                    
                    return module.ResultTaskInternal;
                })
                .ProcessInParallel();
        }
        catch (Exception e) when (ModuleRunType == ModuleRunType.AlwaysRun)
        {
            Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
        }
    }

    private void LogModuleStatus()
    {
        var moduleName = GetType().Name;

        switch (Status)
        {
            case Status.NotYetStarted:
                Context.Logger.LogWarning("Module {Module} never started", moduleName);
                break;
            case Status.Processing:
                Context.Logger.LogError("Module {Module} didn't finish executing", moduleName);
                break;
            case Status.Successful:
                Context.Logger.LogInformation("Module {Module} completed successfully", moduleName);
                break;
            case Status.Failed:
                Context.Logger.LogError("Module {Module} failed", moduleName);
                break;
            case Status.TimedOut:
                Context.Logger.LogError("Module {Module} timed out", moduleName);
                break;
            case Status.Skipped:
                Context.Logger.LogWarning("Module {Module} skipped", moduleName);
                break;
            case Status.Unknown:
                Context.Logger.LogError("Unknown {Module} module status", moduleName);
                break;
            case Status.IgnoredFailure:
                Context.Logger.LogError("Module {Module} failed but the failure was ignored, so this will not cause the pipeline to error", moduleName);
                break;
            case Status.PipelineTerminated:
                Context.Logger.LogError("The pipeline has errored so Module {Module} will terminate", moduleName);
                break;
            case Status.UsedHistory:
                Context.Logger.LogError("Module {Module} has been constructed from historical data", moduleName);
                break;
            default:
                Context.Logger.LogError("Module {Module} status is: {Status}", moduleName, Status);
                break;
        }
    }

    private async Task SaveResult(ModuleResult<T> moduleResult)
    {
        try
        {
            Context.Logger.LogDebug("Saving module result");
            Result = moduleResult;
            ModuleResultTaskCompletionSource.TrySetResult(moduleResult);
            await Context.ModuleResultRepository.SaveResultAsync(this, moduleResult, Context);
        }
        catch (Exception e)
        {
            Context.Logger.LogError(e, "Error saving module result to repository");
        }
    }

    private async Task<T?> ExecuteInternal(Task timeoutExceptionTask)
    {
        var executeAsyncTask = RetryPolicy.ExecuteAsync(() => ExecuteAsync(Context, ModuleCancellationTokenSource.Token));

        // Will throw a timeout exception if configured and timeout is reached
        await Task.WhenAny(timeoutExceptionTask, executeAsyncTask);

        return await executeAsyncTask;
    }
}
