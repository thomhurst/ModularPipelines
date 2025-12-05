using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Represents the execution lifecycle state of a module.
/// </summary>
internal enum ModuleExecutionState
{
    /// <summary>
    /// Module is pending, ready to be queued when dependencies are satisfied.
    /// </summary>
    Pending,

    /// <summary>
    /// Module has been queued to the ready channel, awaiting execution.
    /// </summary>
    Queued,

    /// <summary>
    /// Module is currently executing.
    /// </summary>
    Executing,

    /// <summary>
    /// Module has completed execution.
    /// </summary>
    Completed,
}

/// <summary>
/// Tracks the execution state of a module for eager parallel scheduling.
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
    public ModuleState(IModule module, Type moduleType)
    {
        Module = module;
        ModuleType = moduleType;
        CompletionSource = new TaskCompletionSource<IModule>();
        UnresolvedDependencies = new HashSet<Type>();
        DependentModules = new List<ModuleState>();
        RequiredLockKeys = Array.Empty<string>();
    }

    /// <summary>
    /// Gets the module being tracked.
    /// </summary>
    public IModule Module { get; }

    /// <summary>
    /// Gets the concrete type of the module.
    /// </summary>
    public Type ModuleType { get; }

    /// <summary>
    /// Gets completion source to signal when module execution finishes.
    /// </summary>
    public TaskCompletionSource<IModule> CompletionSource { get; }

    /// <summary>
    /// Gets set of dependency types that haven't completed yet.
    /// </summary>
    public HashSet<Type> UnresolvedDependencies { get; }

    /// <summary>
    /// Gets modules that depend on this module (reverse dependencies).
    /// </summary>
    public List<ModuleState> DependentModules { get; }

    /// <summary>
    /// Gets or sets the current execution state of this module.
    /// </summary>
    public ModuleExecutionState State { get; set; } = ModuleExecutionState.Pending;

    /// <summary>
    /// Gets or sets when the module was queued (for metrics).
    /// </summary>
    public DateTimeOffset? QueuedTime { get; set; }

    /// <summary>
    /// Gets or sets when the module started executing (for metrics).
    /// </summary>
    public DateTimeOffset? ExecutionStartTime { get; set; }

    /// <summary>
    /// Gets or sets when the module completed (for metrics).
    /// </summary>
    public DateTimeOffset? CompletionTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether whether this module must run sequentially (no other modules executing).
    /// </summary>
    public bool RequiresSequentialExecution { get; set; }

    /// <summary>
    /// Gets or sets lock keys that this module requires (for keyed NotInParallel constraints).
    /// </summary>
    public string[] RequiredLockKeys { get; set; }

    /// <summary>
    /// Gets a value indicating whether checks if this module is ready to execute (all dependencies resolved and constraints satisfied)
    /// Note: Constraint checking is performed by the scheduler, this only checks basic readiness.
    /// </summary>
    public bool IsReadyToExecute => UnresolvedDependencies.Count == 0 && State == ModuleExecutionState.Pending;

    /// <summary>
    /// Gets or sets the module result after execution completes.
    /// </summary>
    public IModuleResult? Result { get; set; }

    /// <summary>
    /// Gets or sets the skip decision if the module was skipped.
    /// </summary>
    public SkipDecision SkipResult { get; set; } = SkipDecision.DoNotSkip;
}
