using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public abstract class Module : Module<IDictionary<string, object>>
{
}

public abstract class Module<T> : ModuleBase<T>
{
    private readonly T? _historicResult;
    private readonly Stopwatch _stopwatch = new();
    
    private readonly List<Type> _dependentModules = new();
    
    private IModuleContext _context = null!; // Late Initialisation

    protected Module()
    {
        foreach (var customAttribute in GetType().GetCustomAttributes<DependsOnAttribute>(true))
        {
            AddDependency(customAttribute.Type);
        }
    }

    internal Module(T historicResult) : this()
    {
        _historicResult = historicResult;
    }

    private void AddDependency(Type type)
    {
        if (type == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot depend on itself");
        }

        if (!type.IsAssignableTo(typeof(ModuleBase)))
        {
            throw new Exception($"{type.FullName} must be a module to add as a dependency");
        }
        
        _dependentModules.Add(type);
    }

    protected virtual Task OnBeforeExecute(IModuleContext context)
    {
        return Task.CompletedTask;
    }

    private void CheckDependencyConflicts()
    {
        foreach (var dependentModule in _dependentModules)
        {
            _context.DependencyCollisionDetector.CheckDependency(GetType(), dependentModule);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal override async Task StartAsync(IServiceProvider serviceProvider)
    {
        CreateContext(serviceProvider);

        var pipelineSetupExecutor = _context.Get<IPipelineSetupExecutor>()!;
        
        try
        {
            CheckDependencyConflicts();
            
            await WaitForModuleDependencies();

            if (await ShouldSkip(_context))
            {
                if (await CanRunFromHistory(_context) && _historicResult != null)
                {
                    StartTask.Start(TaskScheduler.Default);
                    TaskCompletionSource.SetResult(_historicResult);
                    return;
                }
                
                Status = Status.Ignored;
                IgnoreTask.Start(TaskScheduler.Default);
                TaskCompletionSource.SetResult(ModuleResult.Empty<T>());

                _context.Logger.LogInformation("{Module} Ignored", GetType().Name);
                
                return;
            }
            
            await pipelineSetupExecutor.OnBeforeModuleStartAsync(this);
            
            await OnBeforeExecute(_context);

            StartTask.Start(TaskScheduler.Default);

            Status = Status.Processing;
            StartTime = DateTimeOffset.UtcNow;

            var timeoutExceptionTask = Task.CompletedTask;
            if (Timeout != TimeSpan.Zero)
            {
                CancellationTokenSource.CancelAfter(Timeout);
                timeoutExceptionTask = Task.Delay(Timeout + TimeSpan.FromSeconds(30), CancellationTokenSource.Token);
            }
            
            _stopwatch.Start();

            var executeAsyncTask = ExecuteAsync(_context, CancellationTokenSource.Token);
            
            // Will throw a timeout exception if configured and timeout is reached
            await Task.WhenAny(timeoutExceptionTask, executeAsyncTask);
            
            var values = await executeAsyncTask ?? ModuleResult.Empty<T>();
            
            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;
            
            Status = Status.Successful;
            EndTime = DateTimeOffset.UtcNow;
            
            _context.Logger.LogDebug("Module Succeeded after {Duration}", Duration);

            TaskCompletionSource.SetResult(values);
        }
        catch (Exception exception)
        {
            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            
            _context.Logger.LogError(exception, "Module Failed after {Duration}", Duration);

            if (exception is TaskCanceledException or OperationCanceledException
                && CancellationTokenSource.IsCancellationRequested)
            {
                _context.Logger.LogDebug("Module timed out {ModuleType}", GetType().FullName);

                Status = Status.TimedOut;
            }
            else
            {
                Status = Status.Failed;
            }

            if (await ShouldIgnoreFailures(_context, exception))
            {
                TaskCompletionSource.SetResult(ModuleResult.FromException<T>(exception));
            }
            else
            {
                TaskCompletionSource.SetException(exception);
                throw;
            }
        }
        finally
        {
            await OnAfterExecute(_context);
            
            await pipelineSetupExecutor.OnAfterModuleEndAsync(this);
        }
    }

    private void CreateContext(IServiceProvider serviceProvider)
    {
        var moduleContextType = typeof(IModuleContext<>).MakeGenericType(GetType());
        _context = (IModuleContext)serviceProvider.GetRequiredService(moduleContextType);
    }

    protected virtual Task OnAfterExecute(IModuleContext context)
    {
        return Task.CompletedTask;
    }
    
    protected TModule GetModule<TModule>() where TModule : ModuleBase
    {
        if (typeof(TModule) == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot get itself");
        }

        _context.DependencyCollisionDetector.CheckDependency(GetType(), typeof(TModule));

        return _context.GetModule<TModule>();
    }
    
    protected internal async Task WaitForModule<TModule>() where TModule : ModuleBase
    {
        var module = GetModule<TModule>();

        _context.Logger.LogDebug("Waiting for Module {ModuleType}", typeof(TModule).FullName);

        await module.Task;
        
        _context.Logger.LogDebug("Finished waiting for Module {ModuleType}", typeof(TModule).FullName);
    }

    private async Task WaitForModuleDependencies()
    {
        if (!_dependentModules.Any())
        {
            return;
        }

        var modules = _dependentModules.Select(_context.GetModule).ToList();

        var tasks = modules.Select(module => module.Task);
        
        await Task.WhenAll(tasks);
    }
}