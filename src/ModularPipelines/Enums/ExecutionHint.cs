using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// Hints about the resource usage characteristics of a module.
/// Used by the scheduler to apply appropriate concurrency limits.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ExecutionHint>))]
public enum ExecutionHint
{
    /// <summary>
    /// Default execution hint. No specific resource constraints applied.
    /// </summary>
    Default = 0,

    /// <summary>
    /// CPU-bound module. Performs heavy computation.
    /// Limited by <see cref="Options.ConcurrencyOptions.MaxCpuIntensiveModules"/>.
    /// </summary>
    CpuBound = 1,

    /// <summary>
    /// I/O-bound module. Performs network calls, file operations, or database queries.
    /// Limited by <see cref="Options.ConcurrencyOptions.MaxIoIntensiveModules"/>.
    /// </summary>
    IoBound = 2,
}
