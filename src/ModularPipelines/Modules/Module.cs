using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.Modules;

public abstract class Module : Module<IDictionary<string, object>>
{
}

/// <summary>
/// The base class from which all custom modules should inherit.
/// </summary>
/// <typeparam name="T">The type of result object this module will return from its ExecuteAsync method.</typeparam>
public abstract partial class Module<T> : ModuleBase<T>
{
    private readonly Stopwatch _stopwatch = new();

    internal List<DependsOnAttribute> DependentModules { get; } = new();

    protected Module()
    {
        foreach (var customAttribute in GetType().GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            AddDependency(customAttribute);
        }
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

        DependentModules.Add(dependsOnAttribute);
    }

    internal override async Task StartAsync()
    {
        if (ModuleRunType != ModuleRunType.AlwaysRun)
        {
            Context.EngineCancellationToken.Token.Register(ModuleCancellationTokenSource.Cancel);
        }

        try
        {
            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            try
            {
                await WaitForModuleDependencies();
            }
            catch when (Context.EngineCancellationToken.IsCancellationRequested && ModuleRunType == ModuleRunType.OnSuccessfulDependencies)
            {
                // The Engine has requested a cancellation due to failures - So fail fast and don't repeat exceptions thrown by other modules.
                ModuleResultTaskCompletionSource.TrySetCanceled();
                return;
            }

            SkipDecision = await ShouldSkip(Context);

            if (SkipDecision.ShouldSkip && await UseResultFromHistoryIfSkipped(Context))
            {
                await SetupModuleFromHistory(SkipDecision.Reason);
                return;
            }

            if (SkipDecision.ShouldSkip)
            {
                SetSkipped(SkipDecision);
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

            ModuleResultTaskCompletionSource.SetResult(moduleResult);
            
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
                var moduleResult = new ModuleResult<T>(exception, this);

                Status = Status.IgnoredFailure;

                await Context.ModuleResultRepository.SaveResultAsync(this, moduleResult);

                ModuleResultTaskCompletionSource.SetResult(moduleResult);
            }
            else
            {
                Context.EngineCancellationToken.Cancel();

                // Give time for Engine to request cancellation
                await Task.Delay(500);
                
                ModuleResultTaskCompletionSource.SetException(exception);
                
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
    
    private async Task<T?> ExecuteInternal(Task timeoutExceptionTask)
    {
        var executeAsyncTask = RetryPolicy.ExecuteAsync(() => ExecuteAsync(Context, ModuleCancellationTokenSource.Token));

        // Will throw a timeout exception if configured and timeout is reached
        await Task.WhenAny(timeoutExceptionTask, executeAsyncTask);

        return await executeAsyncTask;
    }

    private async Task SaveResult(ModuleResult<T> moduleResult)
    {
        try
        {
            await Context.ModuleResultRepository.SaveResultAsync(this, moduleResult);
        }
        catch (Exception e)
        {
            Context.Logger.LogError(e, "Error saving module result to repository");
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
            default:
                Context.Logger.LogError("Module {Module} status is: {Status}", moduleName, Status);
                break;
        }
    }

    internal override ModuleBase Initialize(IPipelineContext context)
    {
        context.FetchLogger(GetType());
        Context = context;
        return this;
    }

    internal override async Task SetupModuleFromHistory(string? skipDecisionReason)
    {
        Status = Status.Successful;

        var result = await Context.ModuleResultRepository.GetResultAsync<T>(this);

        if (result == null)
        {
            SetSkipped(skipDecisionReason ?? "No history for module was found");
            return;
        }

        var utcNow = DateTimeOffset.UtcNow;

        StartTime = utcNow;
        EndTime = utcNow;

        StartTask.Start(TaskScheduler.Default);
        ModuleResultTaskCompletionSource.SetResult(result);
    }

    protected TModule GetModule<TModule>() where TModule : ModuleBase
    {
        var module = GetModuleIfRegistered<TModule>();

        if (module is null)
        {
            throw new ModuleNotRegisteredException(
                $"The module {typeof(TModule)} has not been registered", null);
        }

        return module;
    }

    protected TModule? GetModuleIfRegistered<TModule>() where TModule : ModuleBase
    {
        if (typeof(TModule) == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot get itself");
        }

        return Context.GetModule<TModule>();
    }

    private async Task WaitForModuleDependencies()
    {
        if (!DependentModules.Any())
        {
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
                    return Task.CompletedTask;
                }

                if (module is null)
                {
                    throw new ModuleNotRegisteredException(
                        $"The module {dependsOnAttribute.Type.Name} has not been registered", null);
                }

                return module.ResultTaskInternal;
            })
                .ProcessInParallel();
        }
        catch (Exception e) when (ModuleRunType == ModuleRunType.AlwaysRun)
        {
            Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
        }
    }

    internal override void SetSkipped(SkipDecision skipDecision)
    {
        Status = Status.Skipped;

        SkipDecision = skipDecision;

        SkippedTask.Start(TaskScheduler.Default);

        ModuleResultTaskCompletionSource.SetResult(new SkippedModuleResult<T>(this));

        Context.Logger.LogInformation("{Module} ignored because: {Reason}", GetType().Name, skipDecision.Reason);
    }
}
