using System.Collections.Immutable;
using ModularPipelines.Enums;
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
/// Thread Safety: State and mutable collections are accessed under lock by ModuleScheduler.
/// Dependencies are published as immutable snapshots for lock-free worker reads.
/// </remarks>
internal class ModuleState
{
    private ImmutableDictionary<Type, bool> _dependencies = ImmutableDictionary<Type, bool>.Empty;
    private int _readyEventsStarted;
    private SkipDecision _skipResult = SkipDecision.DoNotSkip;

    public ModuleState(IModule module, Type moduleType, IModuleScheduler? scheduler = null)
    {
        Module = module;
        ModuleType = moduleType;
        Scheduler = scheduler;
        CompletionSource = new TaskCompletionSource<IModule>(TaskCreationOptions.RunContinuationsAsynchronously);
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
    /// Gets the engine-owned scheduler for locally planned execution.
    /// Remote worker states intentionally have no scheduler.
    /// </summary>
    public IModuleScheduler? Scheduler { get; }

    /// <summary>
    /// Gets completion source to signal when module execution finishes.
    /// </summary>
    public TaskCompletionSource<IModule> CompletionSource { get; }

    /// <summary>
    /// Gets all dependency types and whether each dependency is optional.
    /// </summary>
    public ImmutableDictionary<Type, bool> Dependencies => Volatile.Read(ref _dependencies);

    /// <summary>
    /// Adds or updates a dependency by publishing a new immutable snapshot.
    /// </summary>
    public void RecordDependency(Type dependencyType, bool optional)
    {
        ImmutableInterlocked.AddOrUpdate(
            ref _dependencies,
            dependencyType,
            optional,
            (_, existingOptional) => existingOptional && optional);
    }

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
    /// Gets or sets the execution priority of this module.
    /// Higher priority modules are scheduled before lower priority ones when multiple are ready.
    /// </summary>
    public ModulePriority Priority { get; set; } = ModulePriority.Normal;

    /// <summary>
    /// Gets or sets the resource-usage hint for throttling.
    /// </summary>
    public ExecutionHint ExecutionHint { get; set; } = ExecutionHint.Default;

    /// <summary>
    /// Gets or sets when all dependencies were satisfied and the module became ready.
    /// </summary>
    public DateTimeOffset? ReadyTime { get; set; }

    /// <summary>
    /// Atomically records that the ready events have started.
    /// </summary>
    /// <returns><see langword="true"/> only for the first caller.</returns>
    public bool TryStartReadyEvents() => Interlocked.Exchange(ref _readyEventsStarted, 1) == 0;

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
    /// Gets the first skip decision recorded for the module.
    /// </summary>
    public SkipDecision SkipResult => Volatile.Read(ref _skipResult);

    /// <summary>
    /// Records a skip decision unless another caller already recorded one.
    /// </summary>
    public bool TrySetSkipResult(SkipDecision value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (!value.ShouldSkip)
        {
            return false;
        }

        return ReferenceEquals(
            Interlocked.CompareExchange(ref _skipResult, value, SkipDecision.DoNotSkip),
            SkipDecision.DoNotSkip);
    }
}
