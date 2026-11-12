namespace ModularPipelines.Reporting;

/// <summary>
/// Summarizes machine-wide CPU and memory usage sampled during a pipeline run.
/// </summary>
/// <remarks>
/// Values cover the whole machine, including processes unrelated to the pipeline.
/// Fractions range from 0 to 1.
/// </remarks>
public sealed record ResourceUsage
{
    /// <summary>
    /// Gets the highest sampled fraction of machine CPU in use.
    /// </summary>
    public double PeakCpu { get; init; }

    /// <summary>
    /// Gets the mean sampled fraction of machine CPU in use.
    /// </summary>
    public double AverageCpu { get; init; }

    /// <summary>
    /// Gets the highest sampled fraction of machine memory in use.
    /// </summary>
    public double PeakMemory { get; init; }

    /// <summary>
    /// Gets the number of samples taken.
    /// </summary>
    public int SampleCount { get; init; }

    /// <summary>
    /// Gets the total time covered by <see cref="Periods"/>, counting overlapping periods once.
    /// </summary>
    public TimeSpan SaturatedDuration { get; init; }

    /// <summary>
    /// Gets the recorded saturation periods in the order they started. At most 50 are recorded per run.
    /// </summary>
    public IReadOnlyList<ResourceSaturationPeriod> Periods { get; init; } = [];
}
