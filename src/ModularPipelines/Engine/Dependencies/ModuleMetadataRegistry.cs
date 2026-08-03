using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores resolved module metadata.
/// </summary>
internal class ModuleMetadataRegistry : IModuleMetadataRegistry
{
    private readonly ConcurrentDictionary<(Type, string), object> _metadata = new();
    private readonly ConcurrentDictionary<Type, Lazy<ModuleMetadata>> _finalizedMetadata = new();
    private readonly ConcurrentDictionary<(Type ModuleType, Type AttributeType), Attribute[]> _attributesByType = new();
    private readonly IModuleAttributeEventService _attributeEventService;

    public ModuleMetadataRegistry(IModuleAttributeEventService attributeEventService)
    {
        _attributeEventService = attributeEventService;
    }

    public void SetMetadata(Type moduleType, string key, object value)
    {
        _metadata[(moduleType, key)] = value;
    }

    public T? GetMetadata<T>(Type moduleType, string key)
    {
        if (_metadata.TryGetValue((moduleType, key), out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    public void FinalizeMetadata(Type moduleType, IModule instance)
    {
        var metadata = _finalizedMetadata.GetOrAdd(
            moduleType,
            _ => new Lazy<ModuleMetadata>(
                () => CreateMetadata(moduleType, instance),
                LazyThreadSafetyMode.ExecutionAndPublication));
        _ = metadata.Value;
    }

    /// <inheritdoc />
    public IReadOnlySet<string> GetTags(Type moduleType)
        => _finalizedMetadata.TryGetValue(moduleType, out var meta) ? meta.Value.Tags : FrozenSet<string>.Empty;

    /// <inheritdoc />
    public string? GetCategory(Type moduleType)
        => _finalizedMetadata.TryGetValue(moduleType, out var meta) ? meta.Value.Category : null;

    /// <inheritdoc />
    public bool HasAttribute<TAttribute>(Type moduleType)
        where TAttribute : Attribute
        => GetCachedAttributes(moduleType, typeof(TAttribute)).Length > 0;

    /// <inheritdoc />
    public TAttribute? GetAttribute<TAttribute>(Type moduleType)
        where TAttribute : Attribute
    {
        var attributes = GetCachedAttributes(moduleType, typeof(TAttribute));
        return attributes.Length switch
        {
            0 => null,
            1 => (TAttribute) attributes[0],
            _ => throw new AmbiguousMatchException(
                $"Multiple custom attributes of the same type '{typeof(TAttribute)}' found."),
        };
    }

    /// <inheritdoc />
    public IEnumerable<TAttribute> GetAttributes<TAttribute>(Type moduleType)
        where TAttribute : Attribute
        => GetCachedAttributes(moduleType, typeof(TAttribute)).Cast<TAttribute>();

    private ModuleMetadata CreateMetadata(Type moduleType, IModule instance)
    {
        var configuration = instance.Configuration;
        var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Module<T> configuration combines attributes and fluent metadata.
        // Cached attributes preserve metadata for direct IModule implementations.
        tags.UnionWith(configuration.Tags);
        tags.UnionWith(GetAttributes<ModuleTagAttribute>(moduleType)
            .Select(attribute => attribute.Tag));
        var category = configuration.Category
                       ?? GetAttribute<ModuleCategoryAttribute>(moduleType)?.Category;

        return new ModuleMetadata(tags.ToFrozenSet(), category);
    }

    private Attribute[] GetCachedAttributes(Type moduleType, Type attributeType)
    {
        return _attributesByType.GetOrAdd(
            (moduleType, attributeType),
            key => _attributeEventService.GetAttributes(key.ModuleType)
                .Where(key.AttributeType.IsInstanceOfType)
                .ToArray());
    }

    internal sealed record ModuleMetadata(FrozenSet<string> Tags, string? Category);
}
