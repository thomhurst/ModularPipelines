using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Engine;

/// <summary>
/// Caches module scheduling metadata to avoid repeated reflection lookups.
/// </summary>
internal static class ModuleMetadataCache
{
    private static readonly ConcurrentDictionary<Type, ModuleSchedulingMetadata> Cache = new();

    /// <summary>
    /// Gets the cached scheduling metadata for a module type.
    /// </summary>
    /// <param name="moduleType">The module type to get metadata for.</param>
    /// <returns>The cached metadata containing scheduling attributes.</returns>
    public static ModuleSchedulingMetadata GetMetadata(Type moduleType)
    {
        return Cache.GetOrAdd(moduleType, static t => new ModuleSchedulingMetadata
        {
            NotInParallelAttribute = t.GetCustomAttribute<NotInParallelAttribute>(),
            PriorityAttribute = t.GetCustomAttribute<PriorityAttribute>(),
            ExecutionHintAttribute = t.GetCustomAttribute<ExecutionHintAttribute>(),
        });
    }
}
