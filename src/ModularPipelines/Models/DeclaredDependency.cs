using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents a dependency declared programmatically via <see cref="Modules.IDependencyDeclaration"/>.
/// </summary>
/// <param name="ModuleType">The type of the module being depended on.</param>
/// <param name="Kind">The kind of dependency (Required, Optional, Lazy, Conditional).</param>
/// <param name="IsOptional">Whether this dependency is optional (module runs even if dependency is not registered or skipped).</param>
public readonly record struct DeclaredDependency(
    Type ModuleType,
    DependencyType Kind,
    bool IsOptional)
{
    /// <summary>
    /// Creates a required dependency.
    /// </summary>
    public static DeclaredDependency Required(Type type) =>
        new(type, Enums.DependencyType.Required, false);

    /// <summary>
    /// Creates an optional dependency.
    /// </summary>
    public static DeclaredDependency Optional(Type type) =>
        new(type, Enums.DependencyType.Optional, true);

    /// <summary>
    /// Creates a lazy dependency.
    /// </summary>
    public static DeclaredDependency Lazy(Type type) =>
        new(type, Enums.DependencyType.Lazy, true);

    /// <summary>
    /// Creates a conditional dependency.
    /// </summary>
    public static DeclaredDependency Conditional(Type type) =>
        new(type, Enums.DependencyType.Conditional, false);
}
