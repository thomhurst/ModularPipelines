namespace ModularPipelines.Reporting;

/// <summary>
/// Describes distributed execution efficiency for a pipeline run.
/// </summary>
public sealed record DistributedRunReport
{
    /// <summary>Gets the configured or observed worker count.</summary>
    public int WorkerCount { get; init; }

    /// <summary>Gets utilization across the total worker capacity.</summary>
    public double FleetUtilizationPercentage { get; init; }

    /// <summary>Gets utilization details for each worker.</summary>
    public IReadOnlyList<DistributedWorkerRunReport> Workers { get; init; } = [];

    /// <summary>Gets distributed timing details for each module.</summary>
    public IReadOnlyList<DistributedModuleRunReport> Modules { get; init; } = [];
}
