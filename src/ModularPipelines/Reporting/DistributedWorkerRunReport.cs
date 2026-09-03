namespace ModularPipelines.Reporting;

/// <summary>
/// Describes utilization for one distributed worker.
/// </summary>
public sealed record DistributedWorkerRunReport
{
    /// <summary>Gets the worker index.</summary>
    public int WorkerIndex { get; init; }

    /// <summary>Gets the number of modules executed by the worker.</summary>
    public int ModuleCount { get; init; }

    /// <summary>Gets the time occupied processing assignments.</summary>
    public TimeSpan BusyDuration { get; init; }

    /// <summary>Gets the pipeline duration not spent processing assignments.</summary>
    public TimeSpan IdleDuration { get; init; }

    /// <summary>Gets worker utilization as a percentage.</summary>
    public double UtilizationPercentage { get; init; }
}
