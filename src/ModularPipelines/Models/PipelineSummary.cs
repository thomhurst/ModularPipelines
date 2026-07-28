using System.Text.Json.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

public record PipelineSummary
{
    /// <summary>
    /// Gets the modules that are part of the pipeline.
    /// </summary>
    /// <remarks>
    /// This property is excluded from JSON serialization as interface types cannot be deserialized.
    /// </remarks>
    [JsonIgnore]
    public IReadOnlyList<IModule> Modules { get; private init; }

    /// <summary>
    /// Gets the completed module results.
    /// </summary>
    /// <remarks>
    /// Results are excluded from JSON serialization because their generic success values
    /// cannot be reconstructed through the type-erased <see cref="IModuleResult"/> interface.
    /// </remarks>
    [JsonIgnore]
    public IReadOnlyList<IModuleResult> Results { get; private init; }

    /// <summary>
    /// Gets how long the pipeline took to run.
    /// </summary>
    [JsonInclude]
    public TimeSpan TotalDuration { get; private init; }

    /// <summary>
    /// Gets when the pipeline started.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset Start { get; private init; }

    /// <summary>
    /// Gets when the pipeline finished.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset End { get; private init; }

    /// <summary>
    /// Gets the execution metrics for the pipeline.
    /// Contains parallelism factor, peak concurrency, and efficiency metrics.
    /// </summary>
    [JsonInclude]
    public PipelineMetrics? Metrics { get; private init; }

    /// <summary>
    /// Gets the timeline information for each module.
    /// Contains detailed timing data for when each module was ready, queued, started, and completed.
    /// </summary>
    [JsonInclude]
    public IReadOnlyList<ModuleTimeline>? ModuleTimelines { get; private init; }

    [JsonConstructor]
    internal PipelineSummary(
        IReadOnlyList<IModule> modules,
        IReadOnlyList<IModuleResult> results,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end,
        PipelineMetrics? metrics = null,
        IReadOnlyList<ModuleTimeline>? moduleTimelines = null)
    {
        Modules = modules ?? [];
        Results = results ?? [];
        TotalDuration = totalDuration;
        Start = start;
        End = end;
        Metrics = metrics;
        ModuleTimelines = moduleTimelines;
    }

    /// <summary>
    /// Gets the status of the pipeline.
    /// </summary>
    public Status Status => Results.Any(result =>
        result.ExceptionOrDefault is not null
        && result.ModuleStatus != Status.IgnoredFailure)
        ? Status.Failed
        : Status.Successful;

    /// <summary>
    /// Get the Module of type {T}.
    /// </summary>
    /// <typeparam name="T">The module type to get.</typeparam>
    /// <returns>{T}.</returns>
    public T GetModule<T>()
        where T : IModule
        => Modules.GetModule<T>();

}
