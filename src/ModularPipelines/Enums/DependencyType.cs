using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// Defines the type of dependency between modules.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<DependencyType>))]
internal enum DependencyType
{
    /// <summary>
    /// Required dependency. The dependent module will fail if this dependency is not registered.
    /// </summary>
    Required = 0,

    /// <summary>
    /// Optional dependency. The dependent module will not fail if this dependency is not registered.
    /// </summary>
    Optional = 1,
}
