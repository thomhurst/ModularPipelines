namespace ModularPipelines.Reporting;

/// <summary>
/// Describes a period in which a machine resource stayed at or above its configured threshold.
/// </summary>
public sealed record ResourceSaturationPeriod
{
    /// <summary>
    /// Gets the saturated resource.
    /// </summary>
    public ResourceKind Kind { get; init; }

    /// <summary>
    /// Gets when the first sample at or above the threshold was taken.
    /// </summary>
    public DateTimeOffset Start { get; init; }

    /// <summary>
    /// Gets when usage dropped below the threshold, or when monitoring stopped.
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
