using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Represents the execution lifecycle state of a module
/// </summary>
internal enum ModuleExecutionState
{
    /// <summary>
    /// Module is pending, ready to be queued when dependencies are satisfied
    /// </summary>
    Pending,

    /// <summary>
    /// Module has been queued to the ready channel, awaiting execution
    /// </summary>
    Queued,

    /// <summary>
    /// Module is currently executing
    /// </summary>
    Executing,

    /// <summary>
    /// Module has completed execution
    /// </summary>
    Completed
}

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
/// Thread Safety: State property and collections are accessed under lock by ModuleScheduler.
/// State transitions are atomic via enum assignment.
/// </remarks>
internal class ModuleState
{
    public ModuleState(IModule module)
    {
        Module = module;
        CompletionSource = new TaskCompletionSource<IModule>();
        UnresolvedDependencies = new HashSet<Type>();
        DependentModules = new List<ModuleState>();
        RequiredLockKeys = Array.Empty<string>();
    }

    /// <summary>
    /// The module being tracked
    /// </summary>
    public IModule Module { get; }

    /// <summary>
    /// Completion source to signal when module execution finishes
    /// </summary>
    public TaskCompletionSource<IModule> CompletionSource { get; }

    /// <summary>
    /// Set of dependency types that haven't completed yet
    /// </summary>
    public HashSet<Type> UnresolvedDependencies { get; }

    /// <summary>
    /// Modules that depend on this module (reverse dependencies)
    /// </summary>
    public List<ModuleState> DependentModules { get; }

    /// <summary>
    /// The current execution state of this module
    /// </summary>
    public ModuleExecutionState State { get; set; } = ModuleExecutionState.Pending;

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
    public bool IsReadyToExecute => UnresolvedDependencies.Count == 0 && State == ModuleExecutionState.Pending;
}
