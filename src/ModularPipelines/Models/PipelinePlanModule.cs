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
    /// Gets the result of evaluating selection, category, attribute, fluent, and dependency skip conditions.
    /// </summary>
    public SkipDecision SkipDecision { get; }

    /// <summary>
    /// Gets a value indicating whether this module would be skipped.
    /// </summary>
    public bool ShouldSkip => SkipDecision.ShouldSkip;

    /// <summary>
    /// Gets the estimated module duration. Skipped modules have a zero duration.
    /// </summary>
    public TimeSpan EstimatedDuration { get; }

    internal PipelinePlanModule(
        IModule module,
        string? category,
        SkipDecision skipDecision,
        TimeSpan estimatedDuration)
    {
        Module = module;
        ModuleName = module.GetType().FullName ?? module.GetType().Name;
        Category = category;
        SkipDecision = skipDecision;
        EstimatedDuration = estimatedDuration;
    }
}
