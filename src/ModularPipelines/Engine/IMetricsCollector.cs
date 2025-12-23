using ModularPipelines.Enums;
using ModularPipelines.Models;

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
    /// Records when a module becomes ready (all dependencies satisfied).
    /// </summary>
    void RecordModuleReady(Type moduleType, DateTimeOffset time, ModulePriority priority, ExecutionType executionType);

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
    void RecordModuleCompleted(Type moduleType, DateTimeOffset time, bool success, bool skipped, Status status);

    /// <summary>
    /// Records a snapshot of current concurrency.
    /// </summary>
    void RecordConcurrencySnapshot(int currentConcurrency, DateTimeOffset time);

    /// <summary>
    /// Computes aggregate metrics from collected data.
    /// </summary>
    PipelineMetrics ComputeMetrics(DateTimeOffset pipelineStart, DateTimeOffset pipelineEnd, int maxParallelism);

    /// <summary>
    /// Gets timeline information for all modules.
    /// </summary>
    IReadOnlyList<ModuleTimeline> GetTimelines();
}
