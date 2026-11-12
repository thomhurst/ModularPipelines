namespace ModularPipelines.Reporting;

/// <summary>
/// Describes a sustained period of high resource usage, Linux task stalls, or CPU throttling.
/// </summary>
public sealed record ResourceSaturationPeriod
{
    /// <summary>
    /// Gets the saturated resource.
    /// </summary>
    public ResourceKind Kind { get; init; }

    /// <summary>
    /// Gets when the first saturated sample was taken.
    /// </summary>
    public DateTimeOffset Start { get; init; }

    /// <summary>
    /// Gets when saturation ended, or when monitoring stopped.
    /// </summary>
    public DateTimeOffset End { get; init; }

    /// <summary>
    /// Gets the highest fraction in use, from 0 to 1, sampled during the period.
    /// </summary>
    public double Peak { get; init; }

    /// <summary>
    /// Gets the names of the modules that were running at any sample during the period.
    /// </summary>
    public IReadOnlyList<string> Modules { get; init; } = [];
}
