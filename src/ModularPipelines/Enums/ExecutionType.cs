using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// Hints about the resource usage characteristics of a module.
/// Used by the scheduler to apply appropriate concurrency limits.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ExecutionType>))]
public enum ExecutionType
{
    /// <summary>
    /// Default execution type. No specific resource constraints applied.
    /// </summary>
    Default = 0,

    /// <summary>
    /// CPU-intensive module. Performs heavy computation.
    /// Limited by <see cref="Options.ConcurrencyOptions.MaxCpuIntensiveModules"/>.
    /// </summary>
    CpuIntensive = 1,

    /// <summary>
    /// I/O-intensive module. Performs network calls, file operations, or database queries.
    /// Limited by <see cref="Options.ConcurrencyOptions.MaxIoIntensiveModules"/>.
    /// </summary>
    IoIntensive = 2,
}
