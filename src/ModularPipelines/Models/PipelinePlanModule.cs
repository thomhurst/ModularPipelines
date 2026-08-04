using System.Text.Json.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

/// <summary>
/// Describes a module in a pipeline plan.
/// </summary>
public sealed record PipelinePlanModule
{
    /// <summary>
    /// Gets the module instance.
    /// </summary>
    [JsonIgnore]
    public IModule Module { get; }

    /// <summary>
    /// Gets the module type name.
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// Gets the module category, or <see langword="null"/> when none is configured.
    /// </summary>
    public string? Category { get; }

    /// <summary>
    /// Gets the result of evaluating selection, category, attribute, fluent, and dependency skip conditions,
    /// or <see langword="null"/> when a fluent condition requires module results unavailable during planning.
    /// </summary>
    public SkipDecision? SkipDecision { get; }

    /// <summary>
    /// Gets a value indicating whether all skip conditions could be evaluated while planning.
    /// </summary>
    public bool IsSkipDecisionKnown => SkipDecision is not null;

    /// <summary>
    /// Gets a value indicating whether this module would be skipped.
    /// </summary>
    public bool ShouldSkip => SkipDecision?.ShouldSkip is true;

    /// <summary>
    /// Gets the estimated module duration. Skipped modules have a zero duration.
    /// </summary>
    public TimeSpan EstimatedDuration { get; }

    /// <summary>
    /// Gets a value indicating whether fingerprint caching may satisfy this module during execution.
    /// </summary>
    /// <remarks>
    /// The planner cannot predict a cache hit because fingerprints may include dependency results.
    /// </remarks>
    public bool IsCacheCandidate { get; }

    internal PipelinePlanModule(
        IModule module,
        string? category,
        SkipDecision? skipDecision,
        TimeSpan estimatedDuration,
        bool isCacheCandidate)
    {
        Module = module;
        ModuleName = module.GetType().FullName ?? module.GetType().Name;
        Category = category;
        SkipDecision = skipDecision;
        EstimatedDuration = estimatedDuration;
        IsCacheCandidate = isCacheCandidate;
    }
}
