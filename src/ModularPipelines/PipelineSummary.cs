using System.Text.Json.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;

namespace ModularPipelines;

public sealed record PipelineSummary
{
    /// <summary>
    /// Gets the modules that are part of the pipeline.
    /// </summary>
    /// <remarks>
    /// This property is excluded from JSON serialization as interface types cannot be deserialized.
    /// </remarks>
    [JsonIgnore]
    internal IReadOnlyList<IModule> Modules { get; private init; }

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
    public TimeSpan Duration { get; private init; }

    /// <summary>
    /// Gets when the pipeline started.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset StartTime { get; private init; }

    /// <summary>
    /// Gets when the pipeline finished.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset EndTime { get; private init; }

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

    /// <summary>
    /// Gets the machine-readable report produced for this run, when report processing completed.
    /// </summary>
    [JsonInclude]
    public PipelineRunReport? RunReport { get; internal init; }

    /// <summary>
    /// Gets the module results that failed the pipeline.
    /// </summary>
    /// <remarks>
    /// Includes failed, timed-out, dependency-failed, and canceled results, even without an exception.
    /// Also includes other results with an exception, except <see cref="ModuleStatus.FailureIgnored"/>.
    /// Excluded from JSON serialization for the same reason as
    /// <see cref="Results"/>.
    /// </remarks>
    [JsonIgnore]
    public IReadOnlyList<IModuleResult> Failures => [.. Results.Where(IsFailure)];

    /// <summary>
    /// Gets the module results that failed but whose failures were ignored.
    /// </summary>
    /// <remarks>
    /// Includes every result whose status is <see cref="ModuleStatus.FailureIgnored"/>. Excluded from JSON
    /// serialization for the same reason as <see cref="Results"/>.
    /// </remarks>
    [JsonIgnore]
    public IReadOnlyList<IModuleResult> IgnoredFailures =>
        [.. Results.Where(result => result.Status == ModuleStatus.FailureIgnored)];

    /// <summary>
    /// Gets whether the pipeline completed successfully without any unignored failures.
    /// </summary>
    /// <remarks>
    /// Skipped, cached, and ignored-failure module results do not prevent success.
    /// Incomplete, failed, and canceled runs are not successful.
    /// </remarks>
    [JsonInclude]
    public bool Succeeded
    {
        get => Status == ModuleStatus.Succeeded;
        private init => StatusOverride = value ? ModuleStatus.Succeeded : ModuleStatus.Failed;
    }

    [JsonIgnore]
    internal ModuleStatus? StatusOverride { get; init; }

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
        Duration = totalDuration;
        StartTime = start;
        EndTime = end;
        Metrics = metrics;
        ModuleTimelines = moduleTimelines;
    }

    [JsonConstructor]
    internal PipelineSummary(
        IReadOnlyList<IModuleResult> results,
        TimeSpan duration,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        PipelineMetrics? metrics = null,
        IReadOnlyList<ModuleTimeline>? moduleTimelines = null)
        : this([], results, duration, startTime, endTime, metrics, moduleTimelines)
    {
        // Missing success metadata must not turn a legacy or incomplete document into a successful run.
        StatusOverride = ModuleStatus.Unknown;
    }

    /// <summary>
    /// Gets the status of the pipeline.
    /// </summary>
    [JsonIgnore]
    internal ModuleStatus Status
    {
        get
        {
            if (StatusOverride is { } statusOverride)
            {
                return statusOverride;
            }

            if (Results.Any(IsFailure))
            {
                return ModuleStatus.Failed;
            }

            return Results.Count == Modules.Count
                && Results.All(result => result.Status is ModuleStatus.Succeeded or ModuleStatus.Skipped
                    or ModuleStatus.RestoredFromCache or ModuleStatus.RestoredFromHistory or ModuleStatus.FailureIgnored)
                ? ModuleStatus.Succeeded
                : ModuleStatus.Unknown;
        }
    }

    /// <summary>
    /// Get the Module of type {T}.
    /// </summary>
    /// <typeparam name="T">The module type to get.</typeparam>
    /// <returns>{T}.</returns>
    internal T GetModule<T>()
        where T : IModule
        => Modules.OfType<T>().Single();

    internal static bool IsFailure(IModuleResult result)
        => result.Status is ModuleStatus.Failed or ModuleStatus.TimedOut or ModuleStatus.DependencyFailed or ModuleStatus.Canceled
            || (result.ExceptionOrDefault is not null && result.Status != ModuleStatus.FailureIgnored);
}
