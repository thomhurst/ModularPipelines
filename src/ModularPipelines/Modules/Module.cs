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

    private bool _initialized;

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
        if (!_initialized)
        {
            throw new ModuleNotInitializedException(GetType());
        }

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
                return;
            }

            var shouldSkipModule = await ShouldSkip(Context);

            if (shouldSkipModule && await UseResultFromHistoryIfSkipped(Context))
            {
                await SetupModuleFromHistory();
                return;
            }

            if (shouldSkipModule)
            {
                SetSkipped();
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

            var executeAsyncTask = ExecuteAsync(Context, ModuleCancellationTokenSource.Token);

            // Will throw a timeout exception if configured and timeout is reached
            await Task.WhenAny(timeoutExceptionTask, executeAsyncTask);

            var moduleResult = await executeAsyncTask ?? ModuleResult.Empty<T>();
            moduleResult.ModuleName = GetType().Name;

            await Context.ModuleResultRepository.SaveResultAsync(this, moduleResult);

            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;

            Status = Status.Successful;
            EndTime = DateTimeOffset.UtcNow;

            Context.Logger.LogDebug("Module Succeeded after {Duration}", Duration);

            TaskCompletionSource.SetResult(moduleResult);
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
                var moduleResult = ModuleResult.FromException<T>(exception);
                moduleResult.ModuleName = GetType().Name;

                Status = Status.IgnoredFailure;

                await Context.ModuleResultRepository.SaveResultAsync(this, moduleResult);

                TaskCompletionSource.SetResult(moduleResult);
            }
            else
            {
                Context.EngineCancellationToken.Cancel();

                // Give time for Engine to request cancellation
                await Task.Delay(300);
                
                TaskCompletionSource.SetException(exception);
                
                throw;
            }
        }
        finally
        {
            await OnAfterExecute(Context);

            LogModuleStatus();
        }
    }

    private void LogModuleStatus()
    {
        switch (Status)
        {
            case Status.NotYetStarted:
                Context.Logger.LogWarning("Module never started");
                break;
            case Status.Processing:
                Context.Logger.LogError("Module didn't finish executing");
                break;
            case Status.Successful:
                Context.Logger.LogInformation("Module completed successfully");
                break;
            case Status.Failed:
            case Status.TimedOut:
                Context.Logger.LogError("Module failed");
                break;
            case Status.Skipped:
                Context.Logger.LogWarning("Module skipped");
                break;
            case Status.Unknown:
                Context.Logger.LogError("Unknown module status");
                break;
            default:
                Context.Logger.LogError("Module status is: {Status}", Status);
                break;
        }
    }

    internal override ModuleBase Initialize(IModuleContext context)
    {
        context.FetchLogger(GetType());
        Context = context;
        _initialized = true;
        return this;
    }

    private async Task SetupModuleFromHistory()
    {
        Status = Status.Successful;

        var result = await Context.ModuleResultRepository.GetResultAsync<T>(this);

        if (result == null)
        {
            SetSkipped();
            return;
        }

        var utcNow = DateTimeOffset.UtcNow;

        StartTime = utcNow;
        EndTime = utcNow;

        StartTask.Start(TaskScheduler.Default);
        TaskCompletionSource.SetResult(result);
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
        if (!_initialized)
        {
            throw new ModuleNotInitializedException(GetType());
        }

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

    internal override void SetSkipped()
    {
        Status = Status.Skipped;

        SkippedTask.Start(TaskScheduler.Default);

        TaskCompletionSource.SetResult(new SkippedModuleResult<T>());

        Context.Logger.LogInformation("{Module} Ignored", GetType().Name);
    }
}
