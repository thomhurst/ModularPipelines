using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public abstract class ModuleBase
{
    internal readonly Task StartTask = new Task(() => { });
    internal readonly Task IgnoreTask = new Task(() => { });
    internal abstract Task Task { get; }
    
    internal readonly CancellationTokenSource CancellationTokenSource = new();


    public DateTimeOffset StartTime { get; internal set; }
    public DateTimeOffset EndTime { get; internal set; }

    public TimeSpan Duration { get; internal set; }
    public Status Status { get; internal set; } = Status.NotYetStarted;

    public virtual TimeSpan Timeout => TimeSpan.FromMinutes(30);
    public virtual Task<bool> ShouldIgnoreFailures(IModuleContext context, Exception exception) => Task.FromResult(false);
    public virtual Task<bool> ShouldSkip(IModuleContext context) => Task.FromResult(false);
    public virtual Task<bool> CanRunFromHistory(IModuleContext context) => Task.FromResult(false);
    
    internal abstract Task StartAsync(IServiceProvider serviceProvider);
}

public abstract class ModuleBase<T> : ModuleBase
{
    internal readonly TaskCompletionSource<ModuleResult<T>> TaskCompletionSource = new();

    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        return TaskCompletionSource.Task.GetAwaiter();
    }

    internal override Task Task => TaskCompletionSource.Task;

    protected Task<ModuleResult<T>?> NothingAsync() => Task.FromResult(ModuleResult.Empty<T>())!;
    
    protected abstract Task<ModuleResult<T>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
}