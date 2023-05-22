using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pipeline.NET.Exceptions;

namespace Pipeline.NET;

public abstract class Module : IModule
{
    private readonly ILogger _logger;
    protected readonly IModuleContext Context;
    internal readonly Stopwatch Stopwatch = new();
    private readonly List<Func<IModule>> _dependentModules = new();
    internal readonly ConcurrentBag<Type> _dependencyFor = new();

    protected Module(IModuleContext context)
    {
        Context = context;
        _logger = GetLogger();
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

    private readonly TaskCompletionSource<ModuleResult> _taskCompletionSource = new();

    public virtual Task OnBeforeExecute()
    {
        _logger.LogDebug("OnBeforeExecute triggered");
        
        return Task.CompletedTask;
    }

    internal virtual Task InitialiseInternalAsync()
    {
        _logger.LogDebug("InitialiseInternalAsync triggered");

        return Task.CompletedTask;
    }
    
    public virtual Task InitialiseAsync()
    {
        _logger.LogDebug("InitialiseAsync triggered");

        return Task.CompletedTask;
    }

    public async Task<ModuleResult> StartProcessingModule()
    {
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
            
            var values = await ExecuteAsync(_cancellationTokenSource.Token) ?? new ModuleResult();
            
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

                _taskCompletionSource.SetCanceled(_cancellationTokenSource.Token);
                
                throw;
            }
            
            Status = Status.Failed;
            
            _taskCompletionSource.SetException(exception);
            
            throw;
        }
        finally
        {
            await OnAfterExecute();
        }

        return await _taskCompletionSource.Task;
    }

    public abstract Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken);

    public virtual Task OnAfterExecute()
    {
        _logger.LogDebug("OnAfterExecute triggered");

        return Task.CompletedTask;
    }

    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }

    public Status Status { get; private set; } = Status.Unknown;
    
    public virtual TimeSpan Timeout => TimeSpan.Zero;

    public TimeSpan Duration { get; private set; }

    public async Task<TModule> GetModuleAsync<TModule>() where TModule : IModule
    {
        if (typeof(TModule) == GetType())
        {
            throw new ModuleReferencingSelfException("A module cannot get itself");
        }
        
        Context.DependencyCollisionDetector.CheckDependency(GetType(), typeof(TModule));

        var module = Context.GetModule<TModule>();

        _logger.LogDebug("Waiting for Module {ModuleType}", typeof(TModule).FullName);

        await module;
        
        _logger.LogDebug("Finished waiting for Module {ModuleType}", typeof(TModule).FullName);

        return module;
    }

    public Task WaitForModule<TModule>() where TModule : IModule
    {
        return GetModuleAsync<TModule>();
    }

    public TaskAwaiter<ModuleResult> GetAwaiter()
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
            _dependentModules.Select(async module => await module())
        );
    }
}