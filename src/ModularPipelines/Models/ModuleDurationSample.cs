using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents one attributable module-duration observation from retained run history.
/// </summary>
/// <param name="RunId">The unique pipeline run identifier.</param>
/// <param name="End">When the pipeline run ended.</param>
/// <param name="Status">The module's final status.</param>
/// <param name="Duration">The module's measured duration.</param>
public sealed record ModuleDurationSample(
    string RunId,
    DateTimeOffset End,
    Status Status,
    TimeSpan Duration);
