using System.Collections.Concurrent;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides optimized queries for module state collections
/// </summary>
/// <remarks>
/// IMPORTANT: All methods in this class MUST be called while holding the scheduler's state lock
/// to ensure thread-safe access to module states.
/// </remarks>
internal class ModuleStateQueries
{
    private readonly ConcurrentDictionary<Type, ModuleState> _moduleStates;

    public ModuleStateQueries(ConcurrentDictionary<Type, ModuleState> moduleStates)
    {
        _moduleStates = moduleStates;
    }

    /// <summary>
    /// Checks if all modules have completed execution
    /// </summary>
    public bool AreAllModulesCompleted()
    {
        return _moduleStates.Values.All(m => m.IsCompleted);
    }

    /// <summary>
    /// Checks if any modules are currently active (executing or queued)
    /// </summary>
    public bool AreAnyModulesActive()
    {
        return _moduleStates.Values.Any(m => m.IsExecuting || m.IsQueued);
    }

    /// <summary>
    /// Checks if any modules are pending (waiting to be queued)
    /// </summary>
    public bool AreAnyModulesPending()
    {
        return _moduleStates.Values.Any(m => !m.IsCompleted && !m.IsQueued && !m.IsExecuting);
    }

    /// <summary>
    /// Gets all modules that are pending execution
    /// </summary>
    public IEnumerable<ModuleState> GetPendingModules()
    {
        return _moduleStates.Values.Where(m => !m.IsQueued && !m.IsExecuting && !m.IsCompleted);
    }

    /// <summary>
    /// Gets all modules currently executing
    /// </summary>
    public IEnumerable<ModuleState> GetExecutingModules()
    {
        return _moduleStates.Values.Where(m => m.IsExecuting);
    }

    /// <summary>
    /// Gets all modules that are currently queued but not yet executing
    /// </summary>
    public IEnumerable<ModuleState> GetQueuedModules()
    {
        return _moduleStates.Values.Where(m => m.IsQueued && !m.IsExecuting);
    }

    /// <summary>
    /// Gets pending modules that are not marked as AlwaysRun
    /// </summary>
    public IEnumerable<ModuleState> GetCancellablePendingModules()
    {
        return _moduleStates.Values.Where(m =>
            !m.IsExecuting &&
            !m.IsCompleted &&
            m.Module.ModuleRunType != ModuleRunType.AlwaysRun);
    }

    /// <summary>
    /// Finds a sequential module that is currently active, excluding the specified module
    /// </summary>
    public ModuleState? FindBlockingSequentialModule(ModuleState excludeModule)
    {
        return _moduleStates.Values.FirstOrDefault(m =>
            m != excludeModule &&
            m.RequiresSequentialExecution &&
            (m.IsExecuting || m.IsQueued));
    }

    /// <summary>
    /// Checks if any other module (excluding specified) is currently active
    /// </summary>
    public bool AreOtherModulesActive(ModuleState excludeModule)
    {
        return _moduleStates.Values.Any(m =>
            m != excludeModule &&
            (m.IsExecuting || m.IsQueued));
    }

    /// <summary>
    /// Finds a module with lock key conflicts
    /// </summary>
    public ModuleState? FindModuleWithLockConflict(ModuleState moduleState)
    {
        if (moduleState.RequiredLockKeys.Length == 0)
        {
            return null;
        }

        return _moduleStates.Values.FirstOrDefault(m =>
            m != moduleState &&
            (m.IsExecuting || m.IsQueued) &&
            m.RequiredLockKeys.Intersect(moduleState.RequiredLockKeys).Any());
    }

    /// <summary>
    /// Gets statistics about current module states
    /// </summary>
    public ModuleStateStatistics GetStatistics()
    {
        var states = _moduleStates.Values;
        return new ModuleStateStatistics
        {
            Total = states.Count,
            Queued = states.Count(m => m.IsQueued && !m.IsExecuting),
            Executing = states.Count(m => m.IsExecuting),
            Completed = states.Count(m => m.IsCompleted),
            Pending = states.Count(m => !m.IsQueued && !m.IsExecuting && !m.IsCompleted)
        };
    }
}

/// <summary>
/// Statistics about module execution states
/// </summary>
internal record ModuleStateStatistics
{
    public int Total { get; init; }
    public int Queued { get; init; }
    public int Executing { get; init; }
    public int Completed { get; init; }
    public int Pending { get; init; }
}
