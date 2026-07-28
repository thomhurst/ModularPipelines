using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores metadata for modules set during registration.
/// </summary>
internal class ModuleMetadataRegistry : IModuleMetadataRegistry
{
    private readonly ConcurrentDictionary<(Type, string), object> _metadata = new();
    private readonly ConcurrentDictionary<Type, HashSet<string>> _registrationTags = new();
    private readonly ConcurrentDictionary<Type, string> _registrationCategories = new();
    private readonly ConcurrentDictionary<Type, Lazy<ModuleMetadata>> _finalizedMetadata = new();
    private readonly ConcurrentDictionary<(Type ModuleType, Type AttributeType), Attribute[]> _attributesByType = new();
    private readonly IModuleAttributeEventService _attributeEventService;

    public ModuleMetadataRegistry(IOptions<ModuleRegistrationOptions> registrationOptions)
        : this(registrationOptions, new ModuleAttributeEventService())
    {
    }

    public ModuleMetadataRegistry(
        IOptions<ModuleRegistrationOptions> registrationOptions,
        IModuleAttributeEventService attributeEventService)
    {
        _attributeEventService = attributeEventService;

        // Import tags and categories from registration-time configuration
        var options = registrationOptions.Value;

        foreach (var kvp in options.Tags)
        {
            _registrationTags[kvp.Key] = new HashSet<string>(kvp.Value, StringComparer.OrdinalIgnoreCase);
        }

        foreach (var kvp in options.Categories)
        {
            _registrationCategories[kvp.Key] = kvp.Value;
        }
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

    public void AddRegistrationTags(Type moduleType, IEnumerable<string> tags)
    {
        _registrationTags.AddOrUpdate(
            moduleType,
            _ => new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase),
            (_, existing) =>
            {
                existing.UnionWith(tags);
                return existing;
            });
        _finalizedMetadata.TryRemove(moduleType, out _);
    }

    public void SetRegistrationCategory(Type moduleType, string category)
    {
        _registrationCategories[moduleType] = category;
        _finalizedMetadata.TryRemove(moduleType, out _);
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

        // Module<T> configuration combines attributes, overrides, and fluent metadata.
        // Cached attributes preserve metadata for direct IModule implementations.
        tags.UnionWith(configuration.Tags);
        tags.UnionWith(GetAttributes<ModuleTagAttribute>(moduleType)
            .Select(attribute => attribute.Tag));
        if (instance is ITaggedModule taggedModule)
        {
            tags.UnionWith(taggedModule.Tags);
        }

        // Registration tags supplement module-provided metadata.
        if (_registrationTags.TryGetValue(moduleType, out var regTags))
        {
            tags.UnionWith(regTags);
        }

        // Registration metadata takes precedence over module configuration.
        string? category;
        if (_registrationCategories.TryGetValue(moduleType, out var regCat))
        {
            category = regCat;
        }
        else
        {
            category = configuration.Category
                       ?? (instance as ITaggedModule)?.Category
                       ?? GetAttribute<ModuleCategoryAttribute>(moduleType)?.Category;
        }

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
