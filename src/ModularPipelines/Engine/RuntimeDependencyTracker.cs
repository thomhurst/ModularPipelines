using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Checks runtime waits against both scheduler dependencies and other runtime waits.
/// </summary>
internal sealed class RuntimeDependencyTracker(IReadOnlyDictionary<Type, ModuleState> states)
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

            dependencies[dependencyType] = resultTask;
        }
    }

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
