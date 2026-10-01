using System.Collections.Concurrent;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

/// <summary>
/// Thread-safe implementation of metrics collection for pipeline execution.
/// </summary>
internal class MetricsCollector : IMetricsCollector
{
    private readonly ConcurrentDictionary<Type, ModuleMetricsData> _moduleMetrics = new();
    private DateTimeOffset? _pipelineStartTime;

    public void SetPipelineStartTime(DateTimeOffset time)
    {
        _pipelineStartTime = time;
    }

    public DateTimeOffset? GetPipelineStartTime() => _pipelineStartTime;

    public void RecordModuleInitialized(Type moduleType, ModulePriority priority, ExecutionHint executionHint)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.Priority = priority;
        data.ExecutionHint = executionHint;
    }

    public void RecordModuleReady(Type moduleType, DateTimeOffset time, ModulePriority priority, ExecutionHint executionHint)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.ReadyTime = time;
        data.Priority = priority;
        data.ExecutionHint = executionHint;
    }

    public void RecordModuleQueued(Type moduleType, DateTimeOffset time)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.QueuedTime = time;
    }

    public void RecordModuleStarted(Type moduleType, DateTimeOffset time)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.StartTime = time;
        data.Status = ModuleStatus.Running;
    }

    public void RecordModuleCompleted(Type moduleType, DateTimeOffset time, bool success, bool skipped, ModuleStatus status)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.EndTime = time;
        data.WasSuccessful = status is ModuleStatus.Succeeded or ModuleStatus.RestoredFromHistory or ModuleStatus.RestoredFromCache;
        data.WasSkipped = skipped;
        data.Status = status;
    }

    public void RecordReportedExecutionDuration(Type moduleType, TimeSpan duration)
    {
        if (!_moduleMetrics.TryGetValue(moduleType, out var data)
            || data.StartTime is not { } dispatchTime
            || data.EndTime is not { } endTime
            || duration < TimeSpan.Zero)
        {
            return;
        }

        // Keep every timestamp on this process's clock: the module ends when its result is
        // collected and starts one reported duration earlier, never before it was dispatched.
        var reportedStart = endTime - duration;
        data.StartTime = reportedStart > dispatchTime ? reportedStart : dispatchTime;
    }

    public PipelineMetrics ComputeMetrics(DateTimeOffset pipelineStart, DateTimeOffset pipelineEnd, int maxParallelism) =>
        ComputeMetrics(pipelineStart, pipelineEnd, maxParallelism, _moduleMetrics.Values.ToList());

    public PipelineMetrics ComputeMetrics(
        DateTimeOffset pipelineStart,
        DateTimeOffset pipelineEnd,
        int maxParallelism,
        IReadOnlyCollection<Type> moduleTypes) =>
        ComputeMetrics(pipelineStart, pipelineEnd, maxParallelism, GetModuleData(moduleTypes));

    private static PipelineMetrics ComputeMetrics(
        DateTimeOffset pipelineStart,
        DateTimeOffset pipelineEnd,
        int maxParallelism,
        IReadOnlyList<ModuleMetricsData> moduleData)
    {
        var wallClockDuration = pipelineEnd - pipelineStart;

        // Calculate total module execution time (sequential equivalent)
        var totalModuleExecutionTime = TimeSpan.Zero;
        var successfulCount = 0;
        var failedCount = 0;
        var skippedCount = 0;
        var ignoredFailureCount = 0;
        var pendingCount = 0;
        var processingCount = 0;
        var unknownCount = 0;

        foreach (var data in moduleData)
        {
            if (data.StartTime.HasValue && data.EndTime.HasValue)
            {
                totalModuleExecutionTime += data.EndTime.Value - data.StartTime.Value;
            }

            switch (data.Status)
            {
                case ModuleStatus.Succeeded:
                case ModuleStatus.RestoredFromHistory:
                case ModuleStatus.RestoredFromCache:
                    successfulCount++;
                    break;
                case ModuleStatus.Failed:
                case ModuleStatus.Canceled:
                case ModuleStatus.TimedOut:
                case ModuleStatus.DependencyFailed:
                    failedCount++;
                    break;
                case ModuleStatus.FailureIgnored:
                    ignoredFailureCount++;
                    break;
                case ModuleStatus.Skipped:
                    skippedCount++;
                    break;
                case ModuleStatus.NotStarted:
                    pendingCount++;
                    break;
                case ModuleStatus.Running:
                    processingCount++;
                    break;
                case ModuleStatus.Unknown:
                    unknownCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.Status), data.Status, "Unsupported module status");
            }
        }

        // Calculate parallelism factor
        var parallelismFactor = wallClockDuration.TotalMilliseconds > 0
            ? totalModuleExecutionTime.TotalMilliseconds / wallClockDuration.TotalMilliseconds
            : 1.0;

        var (peakConcurrency, averageConcurrency) = ComputeConcurrency(moduleData);

        // Calculate efficiency (average concurrency vs peak achievable given dependencies)
        // Peak concurrency represents the actual maximum parallelism the dependency graph allows,
        // which is more meaningful than comparing against total module count
        var theoreticalMaxParallelism = Math.Min(maxParallelism, peakConcurrency);
        var efficiency = theoreticalMaxParallelism > 0
            ? averageConcurrency / theoreticalMaxParallelism
            : 1.0;

        return new PipelineMetrics
        {
            ParallelismFactor = Math.Round(parallelismFactor, 2),
            PeakConcurrency = peakConcurrency,
            AverageConcurrency = Math.Round(averageConcurrency, 2),
            TotalModuleExecutionTime = totalModuleExecutionTime,
            WallClockDuration = wallClockDuration,
            Efficiency = Math.Round(Math.Min(efficiency, 1.0), 2),
            TotalModules = moduleData.Count,
            SuccessfulModules = successfulCount,
            FailedModules = failedCount,
            SkippedModules = skippedCount,
            IgnoredFailureModules = ignoredFailureCount,
            PendingModules = pendingCount,
            ProcessingModules = processingCount,
            UnknownModules = unknownCount,
        };
    }

    public IReadOnlyList<ModuleTimeline> GetTimelines() => GetTimelines(_moduleMetrics.Values);

    public IReadOnlyList<ModuleTimeline> GetTimelines(IReadOnlyCollection<Type> moduleTypes) =>
        GetTimelines(GetModuleData(moduleTypes));

    private List<ModuleMetricsData> GetModuleData(IReadOnlyCollection<Type> moduleTypes) =>
        [.. moduleTypes
            .Distinct()
            .Select(moduleType => _moduleMetrics.TryGetValue(moduleType, out var data) ? data : null)
            .OfType<ModuleMetricsData>()];

    private static List<ModuleTimeline> GetTimelines(IEnumerable<ModuleMetricsData> moduleData)
    {
        return [.. moduleData
            .Select(data => new ModuleTimeline
            {
                ModuleName = data.ModuleType.Name,
                ModuleTypeName = ModuleTypeIdentifier.Get(data.ModuleType),
                RuntimeModuleTypeName = ModuleTypeIdentifier.GetRuntime(data.ModuleType),
                Priority = data.Priority,
                ExecutionHint = data.ExecutionHint,
                ReadyTime = data.ReadyTime,
                QueuedTime = data.QueuedTime,
                StartTime = data.StartTime,
                EndTime = data.EndTime,
                DependencyWaitTime = CalculateDependencyWaitTime(data),
                QueueWaitTime = CalculateQueueWaitTime(data),
                ExecutionDuration = CalculateExecutionDuration(data),
                WasSkipped = data.WasSkipped,
                WasSuccessful = data.WasSuccessful,
                Status = data.Status,
            })
            .OrderBy(t => t.StartTime ?? DateTimeOffset.MaxValue)];
    }

    /// <summary>
    /// Sweeps module execution windows to find the peak number of modules running at once and
    /// the average number running, weighted by time, while any module was running.
    /// </summary>
    private static (int Peak, double Average) ComputeConcurrency(IReadOnlyList<ModuleMetricsData> moduleData)
    {
        // Ends sort before starts at the same instant so back-to-back modules do not overlap.
        var changes = moduleData
            .Where(static data => data.StartTime.HasValue && data.EndTime > data.StartTime)
            .SelectMany(static data => new[]
            {
                (Time: data.StartTime!.Value, Delta: 1),
                (Time: data.EndTime!.Value, Delta: -1),
            })
            .OrderBy(static change => change.Time)
            .ThenBy(static change => change.Delta)
            .ToList();

        if (changes.Count == 0)
        {
            return (1, 1.0);
        }

        var peak = 0;
        var running = 0;
        var busyTicks = 0L;
        var weightedTicks = 0.0;
        var previousTime = changes[0].Time;
        foreach (var (time, delta) in changes)
        {
            var elapsedTicks = (time - previousTime).Ticks;
            if (running > 0)
            {
                busyTicks += elapsedTicks;
                weightedTicks += (double) running * elapsedTicks;
            }

            running += delta;
            peak = Math.Max(peak, running);
            previousTime = time;
        }

        return (peak, busyTicks > 0 ? weightedTicks / busyTicks : 1.0);
    }

    private static TimeSpan? CalculateDependencyWaitTime(ModuleMetricsData data)
    {
        // Time from pipeline start to when module became ready
        // This is tracked externally; ReadyTime captures when dependencies were satisfied
        return null; // Would need pipeline start time
    }

    private static TimeSpan? CalculateQueueWaitTime(ModuleMetricsData data)
    {
        if (data.QueuedTime.HasValue && data.StartTime.HasValue)
        {
            return data.StartTime.Value - data.QueuedTime.Value;
        }

        return null;
    }

    private static TimeSpan? CalculateExecutionDuration(ModuleMetricsData data)
    {
        if (data.StartTime.HasValue && data.EndTime.HasValue)
        {
            return data.EndTime.Value - data.StartTime.Value;
        }

        return null;
    }

    private class ModuleMetricsData
    {
        public required Type ModuleType { get; init; }
        public ModulePriority Priority { get; set; }
        public ExecutionHint ExecutionHint { get; set; }
        public DateTimeOffset? ReadyTime { get; set; }
        public DateTimeOffset? QueuedTime { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public bool WasSuccessful { get; set; }
        public bool WasSkipped { get; set; }
        public ModuleStatus Status { get; set; }
    }
}
