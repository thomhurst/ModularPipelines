namespace ModularPipelines.Engine.Resources;

/// <summary>
/// Reads machine-wide CPU and memory counters.
/// </summary>
internal interface IMachineResourceReader
{
    bool TryRead(out MachineResourceReading reading);
}

/// <summary>
/// A point-in-time reading. CPU values are cumulative since boot, in an arbitrary unit shared by both fields.
/// </summary>
internal readonly record struct MachineResourceReading(
    long CpuIdle,
    long CpuTotal,
    long MemoryTotalBytes,
    long MemoryAvailableBytes,
    ResourcePressureCounters? Pressure = null);

/// <summary>
/// Optional Linux cumulative counters. A changed source requires a new baseline.
/// </summary>
internal sealed record ResourcePressureCounters(
    string Source,
    long? ThrottledPeriods,
    long? ThrottledMicroseconds,
    long? CpuStallMicroseconds,
    long? MemoryStallMicroseconds,
    string? CpuPressureSource = null,
    string? MemoryPressureSource = null,
    IReadOnlyList<CgroupCpuThrottlingCounters>? AncestorThrottling = null);

internal sealed record CgroupCpuThrottlingCounters(
    string Source,
    long? ThrottledPeriods,
    long? ThrottledMicroseconds);
