using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

/// <summary>
/// Collects execution metrics for pipeline performance analysis.
/// </summary>
internal interface IMetricsCollector
{
    /// <summary>
    /// Sets the pipeline start time for calculating wait times.
    /// </summary>
    void SetPipelineStartTime(DateTimeOffset time);

    /// <summary>
    /// Gets the pipeline start time.
    /// </summary>
    DateTimeOffset? GetPipelineStartTime();

    /// <summary>
    /// Registers a module before dependency scheduling begins.
    /// </summary>
    void RecordModuleInitialized(Type moduleType, ModulePriority priority, ExecutionHint executionHint);

    /// <summary>
    /// Records when a module becomes ready (all dependencies satisfied).
    /// </summary>
    void RecordModuleReady(Type moduleType, DateTimeOffset time, ModulePriority priority, ExecutionHint executionHint);

    /// <summary>
    /// Records when a module is queued for execution.
    /// </summary>
    void RecordModuleQueued(Type moduleType, DateTimeOffset time);

    /// <summary>
    /// Records when a module starts executing.
    /// </summary>
    void RecordModuleStarted(Type moduleType, DateTimeOffset time);

    /// <summary>
    /// Records when a module completes execution.
    /// </summary>
    void RecordModuleCompleted(Type moduleType, DateTimeOffset time, bool success, bool skipped, ModuleStatus status);

    /// <summary>
    /// Moves a completed module's recorded start time so its duration matches the execution time
    /// reported by the process that ran it, excluding dispatch and queue time. The recorded end
    /// time is kept, so all timestamps stay on this process's clock.
    /// </summary>
    void RecordReportedExecutionDuration(Type moduleType, TimeSpan duration);

    /// <summary>
    /// Computes aggregate metrics from collected data.
    /// </summary>
    PipelineMetrics ComputeMetrics(DateTimeOffset pipelineStart, DateTimeOffset pipelineEnd, int maxParallelism);

    /// <summary>
    /// Gets timeline information for all modules.
    /// </summary>
    IReadOnlyList<ModuleTimeline> GetTimelines();
}
