using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents a dependency declared through module configuration or attributes.
/// </summary>
/// <param name="ModuleType">The type of the module being depended on.</param>
/// <param name="Kind">The kind of dependency (required or optional).</param>
/// <param name="IsOptional">Whether this dependency is optional (module runs even if dependency is not registered or skipped).</param>
internal readonly record struct DeclaredDependency(
    Type ModuleType,
    DependencyType Kind,
    bool IsOptional)
{
    /// <summary>
    /// Creates a required dependency.
    /// </summary>
    /// <returns>A required dependency declaration.</returns>
    public static DeclaredDependency Required(Type type) =>
        new(type, Enums.DependencyType.Required, false);

    /// <summary>
    /// Creates an optional dependency.
    /// </summary>
    /// <returns>An optional dependency declaration.</returns>
    public static DeclaredDependency Optional(Type type) =>
        new(type, Enums.DependencyType.Optional, true);
}
