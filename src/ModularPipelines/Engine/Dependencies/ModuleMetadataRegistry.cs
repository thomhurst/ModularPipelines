using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores metadata for modules set during registration.
/// </summary>
internal class ModuleMetadataRegistry : IModuleMetadataRegistry
{
    private readonly ConcurrentDictionary<(Type, string), object> _metadata = new();
    private readonly ConcurrentDictionary<Type, HashSet<string>> _registrationTags = new();
    private readonly ConcurrentDictionary<Type, string> _registrationCategories = new();
    private readonly ConcurrentDictionary<Type, ModuleMetadata> _finalizedMetadata = new();

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
    }

    public void SetRegistrationCategory(Type moduleType, string category)
    {
        _registrationCategories[moduleType] = category;
    }

    public void FinalizeMetadata(Type moduleType, IModule instance)
    {
        var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 1. From attributes
        tags.UnionWith(moduleType.GetCustomAttributes<ModuleTagAttribute>(inherit: true).Select(a => a.Tag));

        // 2. From override (if ITaggedModule)
        if (instance is ITaggedModule tagged)
        {
            tags.UnionWith(tagged.Tags);
        }

        // 3. From registration
        if (_registrationTags.TryGetValue(moduleType, out var regTags))
        {
            tags.UnionWith(regTags);
        }

        // Category: registration > override > attribute
        string? category = null;
        if (_registrationCategories.TryGetValue(moduleType, out var regCat))
        {
            category = regCat;
        }
        else if (instance is ITaggedModule taggedModule && taggedModule.Category != null)
        {
            category = taggedModule.Category;
        }
        else
        {
            category = moduleType.GetCustomAttribute<ModuleCategoryAttribute>(inherit: true)?.Category;
        }

        _finalizedMetadata[moduleType] = new ModuleMetadata(tags.ToFrozenSet(), category);
    }

    /// <inheritdoc />
    public IReadOnlySet<string> GetTags(Type moduleType)
        => _finalizedMetadata.TryGetValue(moduleType, out var meta) ? meta.Tags : FrozenSet<string>.Empty;

    /// <inheritdoc />
    public string? GetCategory(Type moduleType)
        => _finalizedMetadata.TryGetValue(moduleType, out var meta) ? meta.Category : null;

    /// <inheritdoc />
    public bool HasAttribute<TAttribute>(Type moduleType)
        where TAttribute : Attribute
        => moduleType.GetCustomAttribute<TAttribute>(inherit: true) != null;

    /// <inheritdoc />
    public TAttribute? GetAttribute<TAttribute>(Type moduleType)
        where TAttribute : Attribute
        => moduleType.GetCustomAttribute<TAttribute>(inherit: true);

    /// <inheritdoc />
    public IEnumerable<TAttribute> GetAttributes<TAttribute>(Type moduleType)
        where TAttribute : Attribute
        => moduleType.GetCustomAttributes<TAttribute>(inherit: true);

    internal sealed record ModuleMetadata(FrozenSet<string> Tags, string? Category);
}
