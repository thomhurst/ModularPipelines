using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents a dependency declared programmatically via <see cref="Modules.IDependencyDeclaration"/>.
/// </summary>
/// <param name="DependencyType">The type of the module being depended on.</param>
/// <param name="Type">The type of dependency (Required, Optional, Lazy, Conditional).</param>
/// <param name="IgnoreIfNotRegistered">Whether to ignore this dependency if not registered.</param>
public readonly record struct DeclaredDependency(
    Type DependencyType,
    DependencyType Type,
    bool IgnoreIfNotRegistered)
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
