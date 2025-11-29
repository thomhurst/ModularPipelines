using System.Diagnostics;
using System.Runtime.CompilerServices;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Tracks the execution state of a module, separate from the module definition itself.
/// </summary>
/// <remarks>
/// This class implements the separation of concerns between:
/// - What a module IS (the <see cref="IModule{T}"/> implementation)
/// - How a module is EXECUTED (this context class)
///
/// The module itself remains a clean, stateless definition of work to do.
/// </remarks>
internal class ModuleExecutionContext : IModuleExecutionContext
{
    private readonly TaskCompletionSource<IModuleResult> _resultSource = new();

    public ModuleExecutionContext(IModule module, Type moduleType)
    {
        Module = module;
        ModuleType = moduleType;
        Stopwatch = new Stopwatch();
        ModuleCancellationTokenSource = new CancellationTokenSource();
        SubModules = new List<SubModuleTracker>();
    }

    /// <summary>
    /// Gets the module being executed.
    /// </summary>
    public IModule Module { get; }

    /// <summary>
    /// Gets the concrete type of the module.
    /// </summary>
    public Type ModuleType { get; }

    /// <summary>
    /// Gets or sets the current execution status.
    /// </summary>
    public Status Status { get; set; } = Status.NotYetStarted;

    /// <summary>
    /// Gets or sets when the module started executing.
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Gets or sets when the module finished executing.
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// Gets or sets the total execution duration.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets any exception that occurred during execution.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Gets or sets the skip decision if the module was skipped.
    /// </summary>
    public SkipDecision SkipResult { get; set; } = SkipDecision.DoNotSkip;

    /// <summary>
    /// Gets the stopwatch tracking execution time.
    /// </summary>
    public Stopwatch Stopwatch { get; }

    /// <summary>
    /// Gets the cancellation token source for this module.
    /// </summary>
    public CancellationTokenSource ModuleCancellationTokenSource { get; }

    /// <summary>
    /// Gets the list of sub-module trackers.
    /// </summary>
    public List<SubModuleTracker> SubModules { get; }

    /// <summary>
    /// Gets the task that completes when the module execution finishes.
    /// </summary>
    public Task<IModuleResult> ExecutionTask => _resultSource.Task;

    /// <summary>
    /// Gets whether the module has started executing.
    /// </summary>
    public bool IsStarted { get; private set; }

    private readonly object _startLock = new();

    /// <summary>
    /// Sets the module result as successful.
    /// </summary>
    public void SetResult(IModuleResult result)
    {
        _resultSource.TrySetResult(result);
    }

    /// <summary>
    /// Sets the module result as failed.
    /// </summary>
    public void SetException(Exception exception)
    {
        _resultSource.TrySetException(exception);
    }

    /// <summary>
    /// Sets the module result as cancelled.
    /// </summary>
    public void SetCancelled()
    {
        _resultSource.TrySetCanceled();
    }

    /// <summary>
    /// Marks the module as started and returns true if this was the first call.
    /// </summary>
    public bool TryStart()
    {
        lock (_startLock)
        {
            if (IsStarted)
            {
                return false;
            }

            IsStarted = true;
            return true;
        }
    }

    /// <summary>
    /// Records the end of module execution.
    /// </summary>
    public void RecordEndTime()
    {
        Stopwatch.Stop();
        EndTime = DateTimeOffset.UtcNow;
        Duration = Stopwatch.Elapsed;
    }
}

/// <summary>
/// Typed execution context for modules with a specific result type.
/// </summary>
internal class ModuleExecutionContext<T> : ModuleExecutionContext
{
    private readonly TaskCompletionSource<ModuleResult<T>> _typedResultSource = new();

    public ModuleExecutionContext(IModule<T> module, Type moduleType)
        : base(module, moduleType)
    {
        TypedModule = module;
    }

    /// <summary>
    /// Gets the strongly-typed module.
    /// </summary>
    public IModule<T> TypedModule { get; }

    /// <summary>
    /// Gets the task that completes with the typed result.
    /// </summary>
    public Task<ModuleResult<T>> TypedExecutionTask => _typedResultSource.Task;

    /// <summary>
    /// Gets an awaiter for the typed result.
    /// </summary>
    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        return _typedResultSource.Task.GetAwaiter();
    }

    /// <summary>
    /// Sets the typed result.
    /// </summary>
    public void SetTypedResult(ModuleResult<T> result)
    {
        _typedResultSource.TrySetResult(result);
        SetResult(result);
    }

    /// <summary>
    /// Sets the result as failed with an exception.
    /// </summary>
    public new void SetException(Exception exception)
    {
        _typedResultSource.TrySetException(exception);
        base.SetException(exception);
    }

    /// <summary>
    /// Sets the result as cancelled.
    /// </summary>
    public new void SetCancelled()
    {
        _typedResultSource.TrySetCanceled();
        base.SetCancelled();
    }
}

/// <summary>
/// Interface for module execution context, used for non-generic operations.
/// </summary>
internal interface IModuleExecutionContext
{
    IModule Module { get; }
    Type ModuleType { get; }
    Status Status { get; set; }
    DateTimeOffset StartTime { get; set; }
    DateTimeOffset EndTime { get; set; }
    TimeSpan Duration { get; set; }
    Exception? Exception { get; set; }
    SkipDecision SkipResult { get; set; }
    Stopwatch Stopwatch { get; }
    CancellationTokenSource ModuleCancellationTokenSource { get; }
    List<SubModuleTracker> SubModules { get; }
    Task<IModuleResult> ExecutionTask { get; }
    bool IsStarted { get; }
    bool TryStart();
    void RecordEndTime();
    void SetResult(IModuleResult result);
    void SetException(Exception exception);
    void SetCancelled();
}
