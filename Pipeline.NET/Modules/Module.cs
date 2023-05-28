using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Enums;
using Pipeline.NET.Exceptions;
using Pipeline.NET.Models;

namespace Pipeline.NET.Modules;

public abstract class Module : Module<IDictionary<string, object>>
{
    protected Module(IModuleContext context) : base(context)
    {
    }
}

public abstract class Module<T> : IModule<T>
{
    private readonly ILogger _logger;
    protected readonly IModuleContext Context;
    internal readonly Stopwatch Stopwatch = new();
    private readonly List<Func<IModule>> _dependentModules = new();
    private bool _hasStarted;

    protected Module(IModuleContext context)
    {
        Context = context;
        _logger = GetLogger();
        
        foreach (var customAttribute in GetType().GetCustomAttributes<DependsOnAttribute>(true))
        {
            _dependentModules.Add(() => Context.GetModule(customAttribute.Type));
        }
        
        foreach (var customAttribute in GetType().GetCustomAttributes(typeof(DependsOnAttribute<>), true))
        {
            var type = customAttribute.GetType().GetGenericArguments();
            _dependentModules.Add(() => Context.GetModule(type[0]));
        }
    }

    protected void DependsOn<TModule>() where TModule : IModule
    {
        if (typeof(TModule) == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot depend on itself");
        }
        
        Context.DependencyCollisionDetector.CheckDependency(GetType(), typeof(TModule));
        
        _dependentModules.Add(() => Context.GetModule<TModule>());
    }

    private ILogger GetLogger()
    {
        var currentClass = GetType();

        var loggerForCurrentClass = typeof(ILogger<>).MakeGenericType(currentClass);

        return Context.ServiceProvider.GetService(loggerForCurrentClass) as ILogger
               ?? NullLogger.Instance;
    }

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly TaskCompletionSource<ModuleResult<T>> _taskCompletionSource = new();

    protected virtual Task OnBeforeExecute()
    {
        _logger.LogDebug("OnBeforeExecute triggered");
        
        return Task.CompletedTask;
    }

    internal virtual Task InitialiseInternalAsync()
    {
        _logger.LogDebug("InitialiseInternalAsync triggered");

        return Task.CompletedTask;
    }

    protected virtual Task InitialiseAsync()
    {
        _logger.LogDebug("InitialiseAsync triggered");

        return Task.CompletedTask;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public async Task StartProcessingModule()
    {
        if (_hasStarted)
        {
            return;
        }

        _hasStarted = true;
        
        Status = Status.NotYetStarted;
        
        try
        {
            await InitialiseInternalAsync();
            await InitialiseAsync();

            await WaitForDependents();

            await OnBeforeExecute();

            Status = Status.Processing;
            StartTime = DateTimeOffset.UtcNow;

            if (Timeout != TimeSpan.Zero)
            {
                _cancellationTokenSource.CancelAfter(Timeout);
            }
            
            Stopwatch.Start();
            
            var values = await ExecuteAsync(_cancellationTokenSource.Token) ?? ModuleResult.Empty<T>();
            
            Stopwatch.Stop();
            Duration = Stopwatch.Elapsed;
            
            Status = Status.Successful;
            EndTime = DateTimeOffset.UtcNow;
            
            _logger.LogDebug("Module Succeeded after {Duration}", Duration);

            _taskCompletionSource.SetResult(values);
        }
        catch (Exception exception)
        {
            Stopwatch.Stop();
            Duration = Stopwatch.Elapsed;
            
            _logger.LogError(exception, "Module Failed after {Duration}", Duration);

            if (exception is TaskCanceledException or OperationCanceledException
                && _cancellationTokenSource.IsCancellationRequested)
            {
                _logger.LogDebug("Module timed out {ModuleType}", GetType().FullName);

                Status = Status.TimedOut;
            }
            else
            {
                Status = Status.Failed;
            }

            if (IgnoreFailures)
            {
                _taskCompletionSource.SetResult(ModuleResult.FromException<T>(exception));
            }
            else
            {
                _taskCompletionSource.SetException(exception);
                throw;
            }
        }
        finally
        {
            await OnAfterExecute();
        }
    }


    protected abstract Task<ModuleResult<T>?> ExecuteAsync(CancellationToken cancellationToken);

    protected virtual Task OnAfterExecute()
    {
        _logger.LogDebug("OnAfterExecute triggered");

        return Task.CompletedTask;
    }


    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }

    public Status Status { get; set; } = Status.Unknown;

    public virtual TimeSpan Timeout => TimeSpan.Zero;
    public virtual bool IgnoreFailures => false;
    public virtual bool ShouldSkip => false;
    public TimeSpan Duration { get; private set; }

    protected TModule GetModule<TModule>() where TModule : IModule
    {
        if (typeof(TModule) == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot get itself");
        }
        
        Context.DependencyCollisionDetector.CheckDependency(GetType(), typeof(TModule));

        return Context.GetModule<TModule>();
    }
    
    protected async Task WaitForModule<TModule>() where TModule : IModule
    {
        var module = GetModule<TModule>();

        _logger.LogDebug("Waiting for Module {ModuleType}", typeof(TModule).FullName);

        await module.Task;
        
        _logger.LogDebug("Finished waiting for Module {ModuleType}", typeof(TModule).FullName);
    }

    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        return _taskCompletionSource.Task.GetAwaiter();
    }

    private async Task WaitForDependents()
    {
        if (!_dependentModules.Any())
        {
            return;
        }
        
        await Task.WhenAll(
            _dependentModules.Select(async module => await module().Task)
        );
    }

    public Task Task => _taskCompletionSource.Task;
    public Task<ModuleResult<T>> GetResultAsync() => _taskCompletionSource.Task;

    protected ModuleResult<T> Nothing() => ModuleResult.Empty<T>();
    protected Task<ModuleResult<T>?> NothingAsync() => Task.FromResult(ModuleResult.Empty<T>())!;
}