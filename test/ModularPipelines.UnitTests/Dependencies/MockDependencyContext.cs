using System.Reflection;
using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// A mock implementation of <see cref="IDependencyContext"/> for unit testing dependency attributes.
/// </summary>
internal class MockDependencyContext : IDependencyContext
{
    private readonly Dictionary<Type, HashSet<string>> _tags = new();
    private readonly Dictionary<Type, string> _categories = new();

    /// <summary>
    /// Configures tags for a specific module type.
    /// </summary>
    /// <param name="type">The module type to configure tags for.</param>
    /// <param name="tags">The tags to associate with the module type.</param>
    /// <returns>This instance for method chaining.</returns>
    public MockDependencyContext WithTags(Type type, params string[] tags)
    {
        _tags[type] = new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase);
        return this;
    }

    /// <summary>
    /// Configures the category for a specific module type.
    /// </summary>
    /// <param name="type">The module type to configure the category for.</param>
    /// <param name="category">The category to associate with the module type.</param>
    /// <returns>This instance for method chaining.</returns>
    public MockDependencyContext WithCategory(Type type, string category)
    {
        _categories[type] = category;
        return this;
    }

    /// <inheritdoc />
    public IReadOnlySet<string> GetTags(Type moduleType)
        => _tags.TryGetValue(moduleType, out var tags) ? tags : new HashSet<string>();

    /// <inheritdoc />
    public string? GetCategory(Type moduleType)
        => _categories.TryGetValue(moduleType, out var cat) ? cat : null;

    /// <inheritdoc />
    public bool HasAttribute<TAttribute>(Type moduleType) where TAttribute : Attribute
        => moduleType.GetCustomAttribute<TAttribute>() != null;

    /// <inheritdoc />
    public TAttribute? GetAttribute<TAttribute>(Type moduleType) where TAttribute : Attribute
        => moduleType.GetCustomAttribute<TAttribute>();

    /// <inheritdoc />
    public IEnumerable<TAttribute> GetAttributes<TAttribute>(Type moduleType) where TAttribute : Attribute
        => moduleType.GetCustomAttributes<TAttribute>();
}
