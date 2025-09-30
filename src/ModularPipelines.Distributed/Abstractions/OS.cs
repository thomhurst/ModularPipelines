using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Represents operating system types for distributed execution.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OS
{
    /// <summary>
    /// Windows operating system.
    /// </summary>
    Windows,

    /// <summary>
    /// Linux operating system.
    /// </summary>
    Linux,

    /// <summary>
    /// macOS operating system.
    /// </summary>
    MacOS,

    /// <summary>
    /// Unknown or unspecified operating system.
    /// </summary>
    Unknown,
}
