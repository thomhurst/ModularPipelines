using System.Collections.Concurrent;
using ModularPipelines.Reporting;

namespace ModularPipelines.Distributed;

internal sealed class DistributedTelemetryTracker
{
    private readonly ConcurrentDictionary<string, AssignmentTiming> _assignments = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, ResultTiming> _results = new(StringComparer.Ordinal);

    public void RecordAssignment(ModuleAssignment assignment, TimeSpan publishDuration) =>
        _assignments[assignment.ModuleTypeName] = new AssignmentTiming(
            assignment.EnqueuedAt == default ? assignment.AssignedAt : assignment.EnqueuedAt,
            publishDuration,
            assignment.DependencyResults is { Count: > 0 });

    public void RecordResult(SerializedModuleResult result, DateTimeOffset receivedAt) =>
        _results[result.ModuleTypeName] = new ResultTiming(
            result.WorkerIndex,
            result.CompletedAt,
            receivedAt,
            result.ExecutionTelemetry);

    public DistributedRunReport? CreateReport(
        DateTimeOffset pipelineStart,
        DateTimeOffset pipelineEnd,
        int configuredWorkerCount)
    {
        var modules = _results
            .Select(CreateModuleReport)
            .OfType<DistributedModuleRunReport>()
            .OrderBy(static module => module.EnqueuedAt)
            .ToArray();
        if (modules.Length == 0)
        {
            return null;
        }

        var workerCount = Math.Max(
            Math.Max(1, configuredWorkerCount),
            modules.Max(static module => module.WorkerIndex) + 1);
        var runDuration = NonNegative(pipelineEnd - pipelineStart);
        var workers = Enumerable.Range(0, workerCount)
            .Select(workerIndex => CreateWorkerReport(
                workerIndex,
                modules,
                pipelineStart,
                pipelineEnd,
                runDuration))
            .ToArray();
        var capacity = runDuration.TotalMilliseconds * workerCount;
        var busy = workers.Sum(static worker => worker.BusyDuration.TotalMilliseconds);

        return new DistributedRunReport
        {
            WorkerCount = workerCount,
            FleetUtilizationPercentage = Percentage(busy, capacity),
            Workers = workers,
            Modules = modules,
        };
    }

    private DistributedModuleRunReport? CreateModuleReport(
        KeyValuePair<string, ResultTiming> entry)
    {
        var (moduleTypeName, result) = entry;
        if (result.ExecutionTelemetry is not { } execution)
        {
            return null;
        }

        _assignments.TryGetValue(moduleTypeName, out var assignment);
        var enqueuedAt = assignment?.EnqueuedAt ?? execution.ClaimedAt;
        var queueWait = NonNegative(execution.ClaimedAt - enqueuedAt);
        var executionDuration = NonNegative(
            execution.ExecutionFinishedAt - execution.ExecutionStartedAt);
        var assignmentPublish = assignment?.PublishDuration ?? TimeSpan.Zero;
        var dependencyTransfer = assignment?.HasDependencyResults == true
            ? assignmentPublish
            : TimeSpan.Zero;
        var resultTransfer = NonNegative(result.ReceivedAt - result.CompletedAt);
        var totalOverhead = assignmentPublish
                            + execution.DependencyResultProcessingDuration
                            + execution.ArtifactDownloadDuration
                            + execution.ArtifactUploadDuration
                            + resultTransfer;

        return new DistributedModuleRunReport
        {
            ModuleTypeName = moduleTypeName,
            WorkerIndex = result.WorkerIndex,
            EnqueuedAt = enqueuedAt,
            ClaimedAt = execution.ClaimedAt,
            ExecutionStartedAt = execution.ExecutionStartedAt,
            ExecutionFinishedAt = execution.ExecutionFinishedAt,
            ResultReadyAt = result.CompletedAt,
            QueueWaitDuration = queueWait,
            ExecutionDuration = executionDuration,
            AssignmentPublishDuration = assignmentPublish,
            DependencyResultTransferDuration = dependencyTransfer,
            DependencyResultProcessingDuration = execution.DependencyResultProcessingDuration,
            ArtifactDownloadDuration = execution.ArtifactDownloadDuration,
            ArtifactUploadDuration = execution.ArtifactUploadDuration,
            ResultTransferDuration = resultTransfer,
            TotalOverheadDuration = totalOverhead,
        };
    }

    private static DistributedWorkerRunReport CreateWorkerReport(
        int workerIndex,
        IReadOnlyCollection<DistributedModuleRunReport> modules,
        DateTimeOffset pipelineStart,
        DateTimeOffset pipelineEnd,
        TimeSpan runDuration)
    {
        var workerModules = modules.Where(module => module.WorkerIndex == workerIndex).ToArray();
        var busy = CalculateBusyDuration(workerModules, pipelineStart, pipelineEnd);
        var idle = NonNegative(runDuration - busy);
        return new DistributedWorkerRunReport
        {
            WorkerIndex = workerIndex,
            ModuleCount = workerModules.Length,
            BusyDuration = busy,
            IdleDuration = idle,
            UtilizationPercentage = Percentage(busy.TotalMilliseconds, runDuration.TotalMilliseconds),
        };
    }

    private static TimeSpan CalculateBusyDuration(
        IEnumerable<DistributedModuleRunReport> modules,
        DateTimeOffset pipelineStart,
        DateTimeOffset pipelineEnd)
    {
        var intervals = modules
            .Select(module => (
                Start: module.ClaimedAt < pipelineStart ? pipelineStart : module.ClaimedAt,
                End: module.ResultReadyAt > pipelineEnd ? pipelineEnd : module.ResultReadyAt))
            .Where(static interval => interval.End > interval.Start)
            .OrderBy(static interval => interval.Start)
            .ToArray();
        if (intervals.Length == 0)
        {
            return TimeSpan.Zero;
        }

        var busy = TimeSpan.Zero;
        var currentStart = intervals[0].Start;
        var currentEnd = intervals[0].End;
        foreach (var interval in intervals.Skip(1))
        {
            if (interval.Start <= currentEnd)
            {
                currentEnd = interval.End > currentEnd ? interval.End : currentEnd;
                continue;
            }

            busy += currentEnd - currentStart;
            currentStart = interval.Start;
            currentEnd = interval.End;
        }

        return busy + (currentEnd - currentStart);
    }

    private static TimeSpan NonNegative(TimeSpan duration) =>
        duration < TimeSpan.Zero ? TimeSpan.Zero : duration;

    private static double Percentage(double value, double total) =>
        total <= 0 ? 0 : Math.Round(Math.Clamp(value / total * 100, 0, 100), 2);

    private sealed record AssignmentTiming(
        DateTimeOffset EnqueuedAt,
        TimeSpan PublishDuration,
        bool HasDependencyResults);

    private sealed record ResultTiming(
        int WorkerIndex,
        DateTimeOffset CompletedAt,
        DateTimeOffset ReceivedAt,
        DistributedModuleExecutionTelemetry? ExecutionTelemetry);
}
