using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public abstract class ModuleBase
{
    internal readonly Task StartTask = new(() => { });
    internal readonly Task IgnoreTask = new(() => { });
    internal abstract Task<object> ResultTaskInternal { get; }
    
    internal readonly CancellationTokenSource CancellationTokenSource = new();


    public DateTimeOffset StartTime { get; internal set; }
    public DateTimeOffset EndTime { get; internal set; }

    public TimeSpan Duration { get; internal set; }
    public Status Status { get; internal set; } = Status.NotYetStarted;

    protected virtual TimeSpan Timeout => TimeSpan.FromMinutes(30);
    protected virtual Task<bool> ShouldIgnoreFailures(IModuleContext context, Exception exception) => Task.FromResult(false);
    protected virtual Task<bool> ShouldSkip(IModuleContext context) => Task.FromResult(false);
    protected virtual Task<bool> CanRunFromHistory(IModuleContext context) => Task.FromResult(context.ModuleResultRepository.GetType() != typeof(NoOpModuleResultRepository));
    
    internal abstract Task StartAsync();
    internal abstract void SetSkipped();
    internal abstract ModuleBase Initialize(IModuleContext context);
}

public abstract class ModuleBase<T> : ModuleBase
{
    internal readonly TaskCompletionSource<ModuleResult<T>> TaskCompletionSource = new();

    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        return TaskCompletionSource.Task.GetAwaiter();
    }

    internal override Task<object> ResultTaskInternal => TaskCompletionSource.Task.ContinueWith(t => (object) t.Result);

    protected Task<ModuleResult<T>?> NothingAsync() => Task.FromResult(ModuleResult.Empty<T>())!;
    
    protected abstract Task<ModuleResult<T>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
}