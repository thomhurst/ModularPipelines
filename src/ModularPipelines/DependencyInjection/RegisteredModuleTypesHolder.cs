using System.Collections.Concurrent;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Thread-safe holder for tracking registered module types.
/// Used to track module types even when factory delegates are used for registration.
/// </summary>
internal sealed class RegisteredModuleTypesHolder
{
    private readonly ConcurrentDictionary<Type, byte> _types = new();

    /// <summary>
    /// Registers a module type.
    /// </summary>
    public void Add(Type moduleType)
    {
        _types.TryAdd(moduleType, 0);
    }

    /// <summary>
    /// Gets all registered module types.
    /// </summary>
    public HashSet<Type> GetAll()
    {
        return _types.Keys.ToHashSet();
    }

    /// <summary>
    /// Checks if a module type is registered.
    /// </summary>
    public bool Contains(Type moduleType)
    {
        return _types.ContainsKey(moduleType);
    }
}
