using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents a dependency declared programmatically via <see cref="Modules.IDependencyDeclaration"/>.
/// </summary>
/// <param name="ModuleType">The type of the module being depended on.</param>
/// <param name="Kind">The kind of dependency (Required, Optional, Conditional).</param>
/// <param name="IsOptional">Whether this dependency is optional (module runs even if dependency is not registered or skipped).</param>
public readonly record struct DeclaredDependency(
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

    /// <summary>
    /// Creates a conditional dependency.
    /// </summary>
    /// <returns>A conditional dependency declaration.</returns>
    public static DeclaredDependency Conditional(Type type) =>
        new(type, Enums.DependencyType.Conditional, false);
}
