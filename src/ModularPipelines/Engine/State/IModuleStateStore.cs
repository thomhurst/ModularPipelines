using System.Collections.Immutable;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.State;

/// <summary>
/// Interface for thread-safe module state storage with atomic updates.
/// </summary>
internal interface IModuleStateStore
{
    /// <summary>
    /// Event raised when a module's state changes.
    /// </summary>
    event Action<ModuleStateSnapshot, ModuleStateSnapshot>? StateChanged;

    /// <summary>
    /// Registers a new module with initial Pending state.
    /// </summary>
    ModuleStateSnapshot RegisterModule(
        IModule module,
        Type moduleType,
        ImmutableHashSet<Type> dependencies,
        ImmutableList<Type> dependentModules,
        bool requiresSequentialExecution,
        string[] requiredLockKeys,
        ModulePriority priority,
        ExecutionType executionType);

    /// <summary>
    /// Gets the current state of a module.
    /// </summary>
    ModuleStateSnapshot? GetState(Type moduleType);

    /// <summary>
    /// Gets all module states.
    /// </summary>
    IReadOnlyCollection<ModuleStateSnapshot> GetAllStates();

    /// <summary>
    /// Atomically updates a module's state using compare-and-swap.
    /// </summary>
    ModuleStateSnapshot? TryUpdate(Type moduleType, Func<ModuleStateSnapshot, ModuleStateSnapshot?> updateFunc);

    /// <summary>
    /// Removes a resolved dependency from a module.
    /// </summary>
    ModuleStateSnapshot? ResolveDependency(Type moduleType, Type resolvedDependency);

    /// <summary>
    /// Transitions a module from Pending to Queued state.
    /// </summary>
    ModuleStateSnapshot? TransitionToQueued(Type moduleType);

    /// <summary>
    /// Transitions a module from Queued to Running state.
    /// </summary>
    ModuleStateSnapshot? TransitionToRunning(Type moduleType, CancellationTokenSource cancellationSource);

    /// <summary>
    /// Transitions a module from Queued back to Pending (constraint failure).
    /// </summary>
    ModuleStateSnapshot? RevertToPending(Type moduleType);

    /// <summary>
    /// Transitions a module from Running to Completed state.
    /// </summary>
    ModuleStateSnapshot? TransitionToCompleted(Type moduleType, IModuleResult result);

    /// <summary>
    /// Transitions a module from Running to Failed state.
    /// </summary>
    ModuleStateSnapshot? TransitionToFailed(Type moduleType, Exception exception, IModuleResult result);

    /// <summary>
    /// Transitions a module to Skipped state.
    /// </summary>
    ModuleStateSnapshot? TransitionToSkipped(Type moduleType, SkipDecision skipDecision, IModuleResult result);

    /// <summary>
    /// Transitions a module to Cancelled state.
    /// </summary>
    ModuleStateSnapshot? TransitionToCancelled(Type moduleType);

    /// <summary>
    /// Gets modules that are ready to be queued.
    /// </summary>
    IEnumerable<ModuleStateSnapshot> GetReadyModules();

    /// <summary>
    /// Gets modules in Queued state.
    /// </summary>
    IEnumerable<ModuleStateSnapshot> GetQueuedModules();

    /// <summary>
    /// Gets modules in Running state.
    /// </summary>
    IEnumerable<ModuleStateSnapshot> GetRunningModules();

    /// <summary>
    /// Gets modules in terminal states.
    /// </summary>
    IEnumerable<ModuleStateSnapshot> GetCompletedModules();

    /// <summary>
    /// Gets count of modules by state.
    /// </summary>
    (int Pending, int Queued, int Running, int Completed) GetStateCounts();
}
