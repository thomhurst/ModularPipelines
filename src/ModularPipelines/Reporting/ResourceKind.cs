using System.Text.Json.Serialization;

namespace ModularPipelines.Reporting;

/// <summary>
/// Identifies a monitored machine resource.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ResourceKind>))]
public enum ResourceKind
{
    /// <summary>
    /// Machine-wide CPU.
    /// </summary>
    Cpu,

    /// <summary>
    /// Machine-wide physical memory.
    /// </summary>
    Memory,
}
