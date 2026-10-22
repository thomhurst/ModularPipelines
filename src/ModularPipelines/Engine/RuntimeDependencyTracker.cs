using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Checks runtime waits against scheduler dependencies, other runtime waits, and held constraints.
/// </summary>
internal sealed class RuntimeDependencyTracker(
    IReadOnlyDictionary<Type, ModuleState> states,
    IModuleConstraintEvaluator constraintEvaluator,
    int maxParallelism = int.MaxValue)
{
    private readonly Lock _lock = new();
    private readonly Dictionary<Type, Dictionary<Type, Task>> _waits = [];

    public void RegisterWait(IModule module, IModule dependency, Task resultTask)
    {
        var moduleType = module.GetType();
        var dependencyType = dependency.GetType();

        lock (_lock)
        {
            // Only track instances owned by this scheduler, and results that can still block.
            if (resultTask.IsCompleted || ReferenceEquals(module, dependency)
                || !states.TryGetValue(moduleType, out var moduleState)
                || !ReferenceEquals(moduleState.Module, module)
                || moduleState.CompletionSource.Task.IsCompleted
                || !states.TryGetValue(dependencyType, out var dependencyState)
                || !ReferenceEquals(dependencyState.Module, dependency))
            {
                return;
            }

            var path = FindPath(dependencyType, moduleType);
            if (path is not null)
            {
                path.Insert(0, moduleType);
                throw new CircularDependencyException(
                    $"Circular dependency detected while awaiting a module result: {string.Join(" -> ", path.Select(type => type.Name))}. " +
                    $"Remove the circular wait and declare dependencies with [DependsOn<{dependencyType.Name}>] " +
                    $"or Configure(...).DependsOn<{dependencyType.Name}>() on '{moduleType.Name}' " +
                    "so the pipeline can validate and schedule them before execution.")
                {
                    CycleTypes = path,
                };
            }

            if (!_waits.TryGetValue(moduleType, out var dependencies))
            {
                dependencies = [];
                _waits.Add(moduleType, dependencies);
            }

            dependencies.TryGetValue(dependencyType, out var previousWait);
            dependencies[dependencyType] = resultTask;
            try
            {
                CheckConstraintDeadlock(moduleType, dependencyType);
            }
            catch
            {
                // A caller can catch the diagnostic and continue, so do not retain a rejected wait.
                if (previousWait is null)
                {
                    dependencies.Remove(dependencyType);
                }
                else
                {
                    dependencies[dependencyType] = previousWait;
                }

                throw;
            }
        }
    }

    private void CheckConstraintDeadlock(Type moduleType, Type dependencyType)
    {
        if (!states.Values.Any(state => state.RequiresSequentialExecution
            || state.RequiredLockKeys.Length > 0 || state.ParallelLimiter is not null
            || state.ExecutionHintLimit.HasValue)
            && states.Values.Count(OccupiesExecutionSlot) < maxParallelism)
        {
            return;
        }

        // Simulate possible completions without changing scheduler state. An active module with
        // no pending waits can release its constraints; a pending module needs both its dependencies
        // and enough capacity. Only holders that cannot finish contribute to a deadlock.
        var remaining = states.Values
            .Where(state => state.State != ModuleExecutionState.Completed && !state.CompletionSource.Task.IsCompleted)
            .ToDictionary(state => state.ModuleType);

        while (remaining.ContainsKey(moduleType))
        {
            if (!TryCompleteModules(remaining))
            {
                throw new DependencyCollisionException(
                    $"Module '{moduleType.Name}' is active but blocked on a runtime wait for '{dependencyType.Name}'. " +
                    "The remaining modules cannot make progress because waiting modules hold sequential execution, " +
                    "NotInParallel keys, or parallel limit capacity (including MaxParallelism). " +
                    $"Declare the dependency with [DependsOn<{dependencyType.Name}>] " +
                    $"or Configure(...).DependsOn<{dependencyType.Name}>() on '{moduleType.Name}' " +
                    "so it can be scheduled before the waiting module acquires its constraints.");
            }
        }
    }

    private bool TryCompleteModules(Dictionary<Type, ModuleState> remaining)
    {
        // Pending Ready handlers hold global worker capacity without reserving module constraints.
        // Queued modules reserve constraints even before their runners acquire them.
        var active = remaining.Values
            .Where(state => state.State is ModuleExecutionState.Queued or ModuleExecutionState.Executing)
            .ToArray();
        var executionSlotsFull = remaining.Values.Count(OccupiesExecutionSlot) >= maxParallelism;
        var madeProgress = false;
        foreach (var state in remaining.Values.ToArray())
        {
            if (GetPendingDependencies(state).Any(remaining.ContainsKey))
            {
                continue;
            }

            if (state.State != ModuleExecutionState.Executing
                && ((!state.HoldsExecutionSlot && executionSlotsFull) || !constraintEvaluator.CanQueue(state, active)))
            {
                continue;
            }

            remaining.Remove(state.ModuleType);
            madeProgress = true;
        }

        return madeProgress;
    }

    private static bool OccupiesExecutionSlot(ModuleState state) =>
        state.State == ModuleExecutionState.Executing || state.HoldsExecutionSlot;

    private List<Type>? FindPath(Type start, Type target)
    {
        var parents = new Dictionary<Type, Type?> { [start] = null };
        var pending = new Stack<Type>();
        pending.Push(start);

        while (pending.TryPop(out var moduleType))
        {
            if (moduleType == target)
            {
                var path = new List<Type>();
                for (var current = target; current is not null; current = parents[current])
                {
                    path.Add(current);
                }

                path.Reverse();
                return path;
            }

            if (!states.TryGetValue(moduleType, out var state) || state.CompletionSource.Task.IsCompleted)
            {
                continue;
            }

            foreach (var dependency in GetPendingDependencies(state))
            {
                if (parents.TryAdd(dependency, moduleType))
                {
                    pending.Push(dependency);
                }
            }
        }

        return null;
    }

    private IEnumerable<Type> GetPendingDependencies(ModuleState state)
    {
        foreach (var dependency in state.Dependencies.Keys)
        {
            if (states.TryGetValue(dependency, out var dependencyState)
                && !dependencyState.CompletionSource.Task.IsCompleted)
            {
                yield return dependency;
            }
        }

        if (_waits.TryGetValue(state.ModuleType, out var waits))
        {
            foreach (var (dependency, task) in waits)
            {
                if (!task.IsCompleted)
                {
                    yield return dependency;
                }
            }
        }
    }
}
