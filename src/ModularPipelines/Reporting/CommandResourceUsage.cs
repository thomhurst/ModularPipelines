namespace ModularPipelines.Reporting;

/// <summary>
/// Describes resource usage of one completed external command process on the reporting machine.
/// Descendant processes are excluded.
/// </summary>
public sealed record CommandResourceUsage
{
    /// <summary>
    /// Gets the module that ran the command, or null for commands outside a module context.
    /// </summary>
    public ModuleId? ModuleId { get; init; }

    /// <summary>
    /// Gets the process's total CPU time, or null when unavailable.
    /// </summary>
    public TimeSpan? TotalProcessorTime { get; init; }

    /// <summary>
    /// Gets the process's peak working set in bytes, or null when unavailable.
    /// </summary>
    public long? PeakWorkingSetBytes { get; init; }
}
