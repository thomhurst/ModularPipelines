using System.Collections.Concurrent;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores dynamic dependencies added via attribute event receivers.
/// </summary>
internal class ModuleDependencyRegistry : IModuleDependencyRegistry
{
    private readonly ConcurrentDictionary<Type, HashSet<Type>> _dynamicDependencies = new();
    private readonly object _lock = new();

    public void AddDynamicDependency(Type module, Type dependency)
    {
        lock (_lock)
        {
            var deps = _dynamicDependencies.GetOrAdd(module, _ => new HashSet<Type>());
            deps.Add(dependency);
        }
    }

    public void RemoveDependency(Type module, Type dependency)
    {
        lock (_lock)
        {
            if (_dynamicDependencies.TryGetValue(module, out var deps))
            {
                deps.Remove(dependency);
            }
        }
    }

    public IEnumerable<Type> GetDynamicDependencies(Type module)
    {
        if (_dynamicDependencies.TryGetValue(module, out var deps))
        {
            lock (_lock)
            {
                return deps.ToList();
            }
        }

        return Enumerable.Empty<Type>();
    }

    public bool HasDynamicDependencies(Type module)
    {
        return _dynamicDependencies.TryGetValue(module, out var deps) && deps.Count > 0;
    }
}
