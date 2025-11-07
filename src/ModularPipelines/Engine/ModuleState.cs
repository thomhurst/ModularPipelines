using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Tracks the execution state of a module for eager parallel scheduling
/// </summary>
/// <remarks>
/// This class is used internally by the ModuleScheduler to track:
/// - Dependency resolution status via UnresolvedDependencies
/// - Execution lifecycle (pending → queued → executing → completed)
/// - Timing metrics (queued time, execution start, completion)
/// - Constraint requirements (sequential execution, lock keys)
///
/// Thread Safety: Properties are accessed under lock by ModuleScheduler.
/// State transitions are managed atomically to ensure correctness.
/// </remarks>
internal class ModuleState
{
    public ModuleState(ModuleBase module)
    {
        Module = module;
        CompletionSource = new TaskCompletionSource<ModuleBase>();
        UnresolvedDependencies = new HashSet<Type>();
        DependentModules = new List<ModuleState>();
        RequiredLockKeys = Array.Empty<string>();
    }

    /// <summary>
    /// The module being tracked
    /// </summary>
    public ModuleBase Module { get; }

    /// <summary>
    /// Completion source to signal when module execution finishes
    /// </summary>
    public TaskCompletionSource<ModuleBase> CompletionSource { get; }

    /// <summary>
    /// Set of dependency types that haven't completed yet
    /// </summary>
    public HashSet<Type> UnresolvedDependencies { get; }

    /// <summary>
    /// Modules that depend on this module (reverse dependencies)
    /// </summary>
    public List<ModuleState> DependentModules { get; }

    /// <summary>
    /// Whether this module has been queued to the ready channel
    /// </summary>
    public bool IsQueued { get; set; }

    /// <summary>
    /// Whether this module is currently executing
    /// </summary>
    public bool IsExecuting { get; set; }

    /// <summary>
    /// Whether this module has completed execution
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// When the module was queued (for metrics)
    /// </summary>
    public DateTimeOffset? QueuedTime { get; set; }

    /// <summary>
    /// When the module started executing (for metrics)
    /// </summary>
    public DateTimeOffset? ExecutionStartTime { get; set; }

    /// <summary>
    /// When the module completed (for metrics)
    /// </summary>
    public DateTimeOffset? CompletionTime { get; set; }

    /// <summary>
    /// Whether this module must run sequentially (no other modules executing)
    /// </summary>
    public bool RequiresSequentialExecution { get; set; }

    /// <summary>
    /// Lock keys that this module requires (for keyed NotInParallel constraints)
    /// </summary>
    public string[] RequiredLockKeys { get; set; }

    /// <summary>
    /// Checks if this module is ready to execute (all dependencies resolved and constraints satisfied)
    /// Note: Constraint checking is performed by the scheduler, this only checks basic readiness
    /// </summary>
    public bool IsReadyToExecute => UnresolvedDependencies.Count == 0 && !IsQueued && !IsExecuting && !IsCompleted;
}
