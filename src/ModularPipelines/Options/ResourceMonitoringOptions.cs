namespace ModularPipelines.Options;

/// <summary>
/// Configures machine-wide CPU and memory monitoring during a pipeline run.
/// </summary>
/// <remarks>
/// Monitoring is supported on Linux, Windows, and macOS. On other platforms it does nothing.
/// </remarks>
public sealed record ResourceMonitoringOptions
{
    /// <summary>
    /// Gets a value indicating whether machine CPU and memory are sampled during the run.
    /// Default: <see langword="true"/>.
    /// </summary>
    public bool Enabled { get; init; } = true;

    /// <summary>
    /// Gets the time between samples. Default: one second.
    /// </summary>
    public TimeSpan SampleInterval { get; init; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets the fraction of machine CPU in use, from 0 to 1, at or above which the machine counts as saturated.
    /// Default: 0.9.
    /// </summary>
    public double CpuThreshold { get; init; } = 0.9;

    /// <summary>
    /// Gets the fraction of machine memory in use, from 0 to 1, at or above which the machine counts as saturated.
    /// Default: 0.9.
    /// </summary>
    public double MemoryThreshold { get; init; } = 0.9;

    /// <summary>
    /// Gets the number of consecutive samples at or above a threshold or with Linux stalls/throttling
    /// before a warning and saturation period are recorded. Default: 3.
    /// </summary>
    public int SustainedSamples { get; init; } = 3;
}
