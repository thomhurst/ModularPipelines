using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// Defines the type of dependency between modules.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<DependencyType>))]
public enum DependencyType
{
    /// <summary>
    /// Required dependency. The dependent module will fail if this dependency is not registered.
    /// </summary>
    Required = 0,

    /// <summary>
    /// Optional dependency. The dependent module will not fail if this dependency is not registered.
    /// </summary>
    Optional = 1,

    /// <summary>
    /// Conditional dependency. Used when a dependency is added based on a runtime condition via
    /// <see cref="Modules.IDependencyDeclaration.DependsOnIf{TModule}(bool)"/>. For dependency resolution,
    /// this behaves as a required dependency (will fail if condition is true and dependency is not registered).
    /// </summary>
    Conditional = 3,
}
