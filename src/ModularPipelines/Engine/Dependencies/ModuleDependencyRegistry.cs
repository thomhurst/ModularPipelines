using System.Collections.Concurrent;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores dynamic dependencies added via attribute event receivers.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// Dependencies are stored in a <see cref="ConcurrentDictionary{TKey,TValue}"/> with
/// internal locking to protect <see cref="HashSet{T}"/> modifications.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class ModuleDependencyRegistry : IModuleDependencyRegistry
{
    private readonly ConcurrentDictionary<Type, HashSet<Type>> _dynamicDependencies = new();
    private readonly object _lock = new();

    /// <summary>
    /// Adds a dynamic dependency between modules.
    /// </summary>
    /// <param name="module">The module that depends on the dependency.</param>
    /// <param name="dependency">The dependency module type.</param>
    /// <remarks>
    /// This method is thread-safe and can be called concurrently.
    /// </remarks>
    public void AddDynamicDependency(Type module, Type dependency)
    {
        lock (_lock)
        {
            var deps = _dynamicDependencies.GetOrAdd(module, _ => new HashSet<Type>());
            deps.Add(dependency);
        }
    }

    /// <summary>
    /// Removes a dynamic dependency between modules.
    /// </summary>
    /// <param name="module">The module that depends on the dependency.</param>
    /// <param name="dependency">The dependency module type to remove.</param>
    /// <remarks>
    /// This method is thread-safe and can be called concurrently.
    /// </remarks>
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

    /// <summary>
    /// Gets all dynamic dependencies for a module.
    /// </summary>
    /// <param name="module">The module to get dependencies for.</param>
    /// <returns>An enumerable of dependency types.</returns>
    /// <remarks>
    /// This method is thread-safe. Returns a snapshot of dependencies at call time.
    /// </remarks>
    public IEnumerable<Type> GetDynamicDependencies(Type module)
    {
        lock (_lock)
        {
            if (_dynamicDependencies.TryGetValue(module, out var deps))
            {
                return deps.ToList();
            }

            return Enumerable.Empty<Type>();
        }
    }

    /// <summary>
    /// Checks if a module has any dynamic dependencies.
    /// </summary>
    /// <param name="module">The module to check.</param>
    /// <returns>True if the module has dynamic dependencies; otherwise, false.</returns>
    /// <remarks>
    /// This method is thread-safe. The result reflects the state at call time.
    /// </remarks>
    public bool HasDynamicDependencies(Type module)
    {
        lock (_lock)
        {
            return _dynamicDependencies.TryGetValue(module, out var deps) && deps.Count > 0;
        }
    }
}
