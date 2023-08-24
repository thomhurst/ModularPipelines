using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public abstract class ModuleBase
{
    protected internal IModuleContext Context = null!; // Late Initialisation
    
    internal readonly Task StartTask = new(() => { });
    internal readonly Task IgnoreTask = new(() => { });
    internal abstract Task<object> ResultTaskInternal { get; }

    internal readonly CancellationTokenSource ModuleCancellationTokenSource = new();

    /// <summary>
    /// The start time of the module
    /// </summary>
    public DateTimeOffset StartTime { get; internal set; }

    /// <summary>
    /// The end time of the module.
    /// </summary>
    public DateTimeOffset EndTime { get; internal set; }

    /// <summary>
    /// The duration of the module. This will be set after the module has finished.
    /// </summary>
    public TimeSpan Duration { get; internal set; }

    /// <summary>
    /// The status of the module.
    /// </summary>
    public Status Status { get; internal set; } = Status.NotYetStarted;

    /// <summary>
    /// A Timeout for the module
    /// </summary>
    protected virtual TimeSpan Timeout => TimeSpan.FromMinutes(30);

    /// <summary>
    /// If true, the pipeline will not fail is this module fails.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual Task<bool> ShouldIgnoreFailures(IModuleContext context, Exception exception) => Task.FromResult(false);

    /// <summary>
    /// If true, this module will not run.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task<bool> ShouldSkip(IModuleContext context) => Task.FromResult(false);

    /// <summary>
    /// If this module is skipped, and this returns true, the result of this module will be reconstructed from the plugged in <see cref="IModuleResultRepository"/>.
    /// If no persisted result can be reconstructed, this module will fail.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task<bool> UseResultFromHistoryIfSkipped(IModuleContext context) => Task.FromResult(context.ModuleResultRepository.GetType() != typeof(NoOpModuleResultRepository));

    public virtual ModuleRunType ModuleRunType => ModuleRunType.OnSuccessfulDependencies;

    internal abstract Task StartAsync();
    internal abstract void SetSkipped();
    internal abstract ModuleBase Initialize(IModuleContext context);

    internal readonly List<SubModuleBase> SubModuleBases = new();

    internal EventHandler<SubModule>? OnSubModuleCreated;

    protected async Task<T> SubModule<T>(string name, Func<Task<T>> action)
    {
        var submodule = new SubModule<T>(GetType(), name, action);
        
        OnSubModuleCreated?.Invoke(this, submodule);
        
        SubModuleBases.Add(submodule);
        
        return await submodule.Task;
    }

    protected async Task SubModule(string name, Func<Task> action)
    {
        var submodule = new SubModule(GetType(), name, action);
        
        OnSubModuleCreated?.Invoke(this, submodule);
        
        SubModuleBases.Add(submodule);
        
        await submodule.Task;
    }
}

public abstract class ModuleBase<T> : ModuleBase
{
    internal readonly TaskCompletionSource<ModuleResult<T>> TaskCompletionSource = new();

    /// <summary>
    /// The awaiter used to return the result of the module when awaited
    /// </summary>
    /// <returns></returns>
    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        return TaskCompletionSource.Task.GetAwaiter();
    }

    internal override Task<object> ResultTaskInternal => TaskCompletionSource.Task.ContinueWith(t => (object) t.Result);

    /// <summary>
    /// Used to return no result in a module
    /// </summary>
    /// <returns></returns>
    protected Task<ModuleResult<T>?> NothingAsync() => Task.FromResult(ModuleResult.Empty<T>())!;

    /// <summary>
    /// The core logic of the module goes here
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<ModuleResult<T>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
}
