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
    /// Lazy dependency. The dependency is only executed if explicitly awaited within the module.
    /// </summary>
    Lazy = 2,

    /// <summary>
    /// Conditional dependency. The dependency is only active if a condition is met.
    /// </summary>
    Conditional = 3,
}
