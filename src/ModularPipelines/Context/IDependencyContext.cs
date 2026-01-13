namespace ModularPipelines.Context;

/// <summary>
/// Provides access to module metadata for dependency resolution.
/// </summary>
public interface IDependencyContext
{
    /// <summary>
    /// Gets the tags declared on a module.
    /// </summary>
    /// <param name="moduleType">The module type to get tags for.</param>
    /// <returns>A read-only set of tags declared on the module.</returns>
    IReadOnlySet<string> GetTags(Type moduleType);

    /// <summary>
    /// Gets the category of a module (null if none).
    /// </summary>
    /// <param name="moduleType">The module type to get the category for.</param>
    /// <returns>The category of the module, or null if none is declared.</returns>
    string? GetCategory(Type moduleType);

    /// <summary>
    /// Checks if a module has a specific attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
    /// <param name="moduleType">The module type to check.</param>
    /// <returns>True if the module has the specified attribute, false otherwise.</returns>
    bool HasAttribute<TAttribute>(Type moduleType) where TAttribute : Attribute;

    /// <summary>
    /// Gets a specific attribute from a module (null if not present).
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
    /// <param name="moduleType">The module type to get the attribute from.</param>
    /// <returns>The attribute instance, or null if not present.</returns>
    TAttribute? GetAttribute<TAttribute>(Type moduleType) where TAttribute : Attribute;

    /// <summary>
    /// Gets all attributes of the specified type from a module.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
    /// <param name="moduleType">The module type to get attributes from.</param>
    /// <returns>An enumerable of all attributes of the specified type.</returns>
    IEnumerable<TAttribute> GetAttributes<TAttribute>(Type moduleType) where TAttribute : Attribute;
}
