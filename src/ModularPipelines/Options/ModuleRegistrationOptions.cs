using System.Collections.Concurrent;

namespace ModularPipelines.Options;

/// <summary>
/// Options for storing module metadata configured during registration.
/// </summary>
public class ModuleRegistrationOptions
{
    /// <summary>
    /// All module types that have been explicitly registered.
    /// Used by ModuleAutoRegistrar to find registered modules when factory delegates are used.
    /// </summary>
    internal ConcurrentDictionary<Type, byte> RegisteredModuleTypes { get; } = new();

    /// <summary>
    /// Tags configured for modules during registration.
    /// Key is the module type, value is the collection of tags.
    /// </summary>
    internal ConcurrentDictionary<Type, HashSet<string>> Tags { get; } = new();

    /// <summary>
    /// Categories configured for modules during registration.
    /// Key is the module type, value is the category.
    /// </summary>
    internal ConcurrentDictionary<Type, string> Categories { get; } = new();

    /// <summary>
    /// Registers a module type as being explicitly registered.
    /// </summary>
    /// <param name="moduleType">The module type being registered.</param>
    internal void RegisterModuleType(Type moduleType)
    {
        RegisteredModuleTypes.TryAdd(moduleType, 0);
    }

    /// <summary>
    /// Adds tags to a module type.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="tags">The tags to add.</param>
    internal void AddTags(Type moduleType, IEnumerable<string> tags)
    {
        Tags.AddOrUpdate(
            moduleType,
            _ => new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase),
            (_, existing) =>
            {
                existing.UnionWith(tags);
                return existing;
            });
    }

    /// <summary>
    /// Sets the category for a module type.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="category">The category to set.</param>
    internal void SetCategory(Type moduleType, string category)
    {
        Categories[moduleType] = category;
    }
}
