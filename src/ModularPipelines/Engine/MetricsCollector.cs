using System.Collections.Concurrent;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Thread-safe implementation of metrics collection for pipeline execution.
/// </summary>
internal class MetricsCollector : IMetricsCollector
{
    private readonly ConcurrentDictionary<Type, ModuleMetricsData> _moduleMetrics = new();
    private readonly ConcurrentBag<ConcurrencySnapshot> _concurrencySnapshots = new();
    private DateTimeOffset? _pipelineStartTime;

    public void SetPipelineStartTime(DateTimeOffset time)
    {
        _pipelineStartTime = time;
    }

    public DateTimeOffset? GetPipelineStartTime() => _pipelineStartTime;

    public void RecordModuleReady(Type moduleType, DateTimeOffset time, ModulePriority priority, ExecutionType executionType)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.ReadyTime = time;
        data.Priority = priority;
        data.ExecutionType = executionType;
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
    }

    public void RecordModuleCompleted(Type moduleType, DateTimeOffset time, bool success, bool skipped, Status status)
    {
        var data = _moduleMetrics.GetOrAdd(moduleType, _ => new ModuleMetricsData { ModuleType = moduleType });
        data.EndTime = time;
        data.WasSuccessful = success;
        data.WasSkipped = skipped;
        data.Status = status;
    }

    public void RecordConcurrencySnapshot(int currentConcurrency, DateTimeOffset time)
    {
        _concurrencySnapshots.Add(new ConcurrencySnapshot(currentConcurrency, time));
    }

    public PipelineMetrics ComputeMetrics(DateTimeOffset pipelineStart, DateTimeOffset pipelineEnd, int maxParallelism)
    {
        var wallClockDuration = pipelineEnd - pipelineStart;
        var moduleData = _moduleMetrics.Values.ToList();

        // Calculate total module execution time (sequential equivalent)
        var totalModuleExecutionTime = TimeSpan.Zero;
        var successfulCount = 0;
        var failedCount = 0;
        var skippedCount = 0;

        foreach (var data in moduleData)
        {
            if (data.StartTime.HasValue && data.EndTime.HasValue)
            {
                totalModuleExecutionTime += data.EndTime.Value - data.StartTime.Value;
            }

            if (data.WasSkipped)
            {
                skippedCount++;
            }
            else if (data.WasSuccessful)
            {
                successfulCount++;
            }
            else
            {
                failedCount++;
            }
        }

        // Calculate parallelism factor
        var parallelismFactor = wallClockDuration.TotalMilliseconds > 0
            ? totalModuleExecutionTime.TotalMilliseconds / wallClockDuration.TotalMilliseconds
            : 1.0;

        // Calculate concurrency metrics
        var snapshots = _concurrencySnapshots.ToList();
        var peakConcurrency = snapshots.Count > 0 ? snapshots.Max(s => s.Concurrency) : 1;
        var averageConcurrency = snapshots.Count > 0 ? snapshots.Average(s => s.Concurrency) : 1.0;

        // Calculate efficiency (actual parallelism vs theoretical max)
        var theoreticalMaxParallelism = Math.Min(maxParallelism, moduleData.Count);
        var efficiency = theoreticalMaxParallelism > 0
            ? parallelismFactor / theoreticalMaxParallelism
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
        };
    }

    public IReadOnlyList<ModuleTimeline> GetTimelines()
    {
        return _moduleMetrics.Values
            .Select(data => new ModuleTimeline
            {
                ModuleName = data.ModuleType.Name,
                ModuleTypeName = data.ModuleType.FullName ?? data.ModuleType.Name,
                Priority = data.Priority,
                ExecutionType = data.ExecutionType,
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
            .OrderBy(t => t.StartTime ?? DateTimeOffset.MaxValue)
            .ToList();
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
        public Type ModuleType { get; set; } = null!;
        public ModulePriority Priority { get; set; }
        public ExecutionType ExecutionType { get; set; }
        public DateTimeOffset? ReadyTime { get; set; }
        public DateTimeOffset? QueuedTime { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public bool WasSuccessful { get; set; }
        public bool WasSkipped { get; set; }
        public Status Status { get; set; }
    }

    private record ConcurrencySnapshot(int Concurrency, DateTimeOffset Time);
}
