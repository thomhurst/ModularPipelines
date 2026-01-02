using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Engine;

/// <summary>
/// Caches module metadata to avoid repeated reflection lookups.
/// </summary>
/// <remarks>
/// This cache provides unified access to both:
/// - Scheduling metadata (attributes like NotInParallel, Priority)
/// - Behavior metadata (interfaces like ISkippable, IHookable)
///
/// Using cached metadata avoids repeated reflection and runtime type checks.
/// </remarks>
internal static class ModuleMetadataCache
{
    private static readonly ConcurrentDictionary<Type, ModuleSchedulingMetadata> SchedulingCache = new();
    private static readonly ConcurrentDictionary<Type, ModuleBehaviorMetadata> BehaviorCache = new();

    /// <summary>
    /// Gets the cached scheduling metadata for a module type.
    /// </summary>
    /// <param name="moduleType">The module type to get metadata for.</param>
    /// <returns>The cached metadata containing scheduling attributes.</returns>
    public static ModuleSchedulingMetadata GetSchedulingMetadata(Type moduleType)
    {
        return SchedulingCache.GetOrAdd(moduleType, static t => new ModuleSchedulingMetadata
        {
            NotInParallelAttribute = t.GetCustomAttribute<NotInParallelAttribute>(),
            PriorityAttribute = t.GetCustomAttribute<PriorityAttribute>(),
            ExecutionHintAttribute = t.GetCustomAttribute<ExecutionHintAttribute>(),
        });
    }

    /// <summary>
    /// Gets the cached behavior metadata for a module type.
    /// </summary>
    /// <param name="moduleType">The module type to get behavior metadata for.</param>
    /// <returns>The cached metadata containing behavior interface flags.</returns>
    public static ModuleBehaviorMetadata GetBehaviorMetadata(Type moduleType)
    {
        return BehaviorCache.GetOrAdd(moduleType, ModuleBehaviorMetadata.FromType);
    }

    /// <summary>
    /// Gets the cached scheduling metadata for a module type.
    /// </summary>
    /// <param name="moduleType">The module type to get metadata for.</param>
    /// <returns>The cached metadata containing scheduling attributes.</returns>
    [Obsolete("Use GetSchedulingMetadata instead for clarity.")]
    public static ModuleSchedulingMetadata GetMetadata(Type moduleType) => GetSchedulingMetadata(moduleType);
}
