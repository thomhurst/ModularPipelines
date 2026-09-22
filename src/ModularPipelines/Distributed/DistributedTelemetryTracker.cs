using System.Collections.Concurrent;
using ModularPipelines.Reporting;

namespace ModularPipelines.Distributed;

internal sealed class DistributedTelemetryTracker
{
    private readonly ConcurrentDictionary<ModuleId, AssignmentTiming> _assignments = new();
    private readonly ConcurrentDictionary<ModuleId, ResultTiming> _results = new();

    public void RecordAssignment(ModuleAssignment assignment, TimeSpan publishDuration) =>
        _assignments[assignment.ModuleId] = new AssignmentTiming(
            assignment.EnqueuedAt,
            publishDuration);

    public void RecordResult(SerializedModuleResult result, DateTimeOffset receivedAt) =>
        _results[result.ModuleId] = new ResultTiming(
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
        KeyValuePair<ModuleId, ResultTiming> entry)
    {
        var (moduleId, result) = entry;
        if (result.ExecutionTelemetry is not { } execution)
        {
            return null;
        }

        _assignments.TryGetValue(moduleId, out var assignment);
        var enqueuedAt = assignment?.EnqueuedAt ?? execution.ClaimedAt;
        var queueWait = NonNegative(execution.ClaimedAt - enqueuedAt);
        var executionDuration = NonNegative(
            execution.ExecutionFinishedAt - execution.ExecutionStartedAt);
        var assignmentPublish = assignment?.PublishDuration ?? TimeSpan.Zero;
        var dependencyTransfer = execution.DependencyResultTransferDuration;
        var resultTransfer = NonNegative(result.ReceivedAt - result.CompletedAt);
        var totalOverhead = assignmentPublish
                            + dependencyTransfer
                            + execution.DependencyResultProcessingDuration
                            + execution.ArtifactDownloadDuration
                            + execution.ArtifactUploadDuration
                            + resultTransfer;

        return new DistributedModuleRunReport
        {
            ModuleTypeName = moduleId.Value,
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
        foreach (var (start, end) in intervals.Skip(1))
        {
            if (start <= currentEnd)
            {
                currentEnd = end > currentEnd ? end : currentEnd;
                continue;
            }

            busy += currentEnd - currentStart;
            currentStart = start;
            currentEnd = end;
        }

        return busy + (currentEnd - currentStart);
    }

    private static TimeSpan NonNegative(TimeSpan duration) =>
        duration < TimeSpan.Zero ? TimeSpan.Zero : duration;

    private static double Percentage(double value, double total) =>
        total <= 0 ? 0 : Math.Round(Math.Clamp(value / total * 100, 0, 100), 2);

    private sealed record AssignmentTiming(
        DateTimeOffset EnqueuedAt,
        TimeSpan PublishDuration);

    private sealed record ResultTiming(
        int WorkerIndex,
        DateTimeOffset CompletedAt,
        DateTimeOffset ReceivedAt,
        DistributedModuleExecutionTelemetry? ExecutionTelemetry);
}
