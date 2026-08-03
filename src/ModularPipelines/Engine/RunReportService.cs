using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class RunReportService(
    IRunHistoryStore historyStore,
    PipelineRunReportFactory reportFactory,
    IBuildSystemDetector buildSystemDetector,
    IOptions<PipelineOptions> pipelineOptions,
    IOptions<DistributedOptions> distributedOptions,
    RoleDetector roleDetector,
    IDistributedCoordinator distributedCoordinator,
    ICommandExecutionCounter commandExecutionCounter,
    ILogger<RunReportService> logger) : IRunReportService
{
    private static readonly TimeSpan HistoryStoreTimeout = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan WorkerMetricsTimeout = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan WorkerMetricsPollingInterval = TimeSpan.FromMilliseconds(100);

    public async Task<PipelineRunReport> CompleteAsync(
        PipelineSummary summary,
        Exception? pipelineException = null)
    {
        var isDistributedWorker = IsDistributedWorker();
        await SynchronizeDistributedMetricsAsync(isDistributedWorker, summary)
            .ConfigureAwait(false);

        var reportPath = isDistributedWorker ? null : GetReportPath();
        var pipelineIdentity = GetPipelineIdentity(summary, reportPath);
        var historyEnabled = reportPath is not null
            && pipelineOptions.Value.RunReport.HistoryRetention > 0;
        var previousReport = await LoadPreviousReportAsync(historyEnabled, pipelineIdentity)
            .ConfigureAwait(false);
        var report = CreateReport(summary, previousReport, pipelineIdentity, pipelineException);

        await WriteReportAsync(reportPath, report).ConfigureAwait(false);
        await SaveHistoryAsync(historyEnabled, report).ConfigureAwait(false);
        return report;
    }

    private async Task SynchronizeDistributedMetricsAsync(
        bool isDistributedWorker,
        PipelineSummary summary)
    {
        if (isDistributedWorker)
        {
            await PublishWorkerMetricsAsync().ConfigureAwait(false);
        }
        else if (IsDistributedMaster())
        {
            await AggregateWorkerMetricsAsync(summary).ConfigureAwait(false);
        }
    }

    private async Task<PipelineRunReport?> LoadPreviousReportAsync(
        bool historyEnabled,
        string pipelineIdentity)
    {
        if (!historyEnabled)
        {
            return null;
        }

        try
        {
            using var timeout = new CancellationTokenSource(HistoryStoreTimeout);
            return await historyStore.GetLatestAsync(pipelineIdentity, timeout.Token).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not load previous pipeline run report");
            return null;
        }
    }

    private PipelineRunReport CreateReport(
        PipelineSummary summary,
        PipelineRunReport? previousReport,
        string pipelineIdentity,
        Exception? pipelineException)
    {
        try
        {
            return reportFactory.Create(
                summary,
                previousReport,
                pipelineIdentity,
                pipelineException);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not create pipeline run report");
            return new PipelineRunReport
            {
                PipelineIdentity = pipelineIdentity,
                Status = pipelineException is null ? summary.Status : Status.Failed,
                Start = summary.Start,
                End = summary.End,
                TotalDuration = summary.TotalDuration,
                Metrics = summary.Metrics,
                Exception = CreateFallbackExceptionDetails(pipelineException),
            };
        }
    }

    private static RunReportExceptionDetails? CreateFallbackExceptionDetails(Exception? exception)
    {
        return exception is null
            ? null
            : new RunReportExceptionDetails
            {
                Type = exception.GetType().FullName ?? exception.GetType().Name,
                Message = "Exception details unavailable because secret obfuscation failed.",
                InnerException = CreateFallbackExceptionDetails(exception.InnerException),
                InnerExceptions = exception is AggregateException aggregateException
                    ? aggregateException.InnerExceptions
                        .Select(CreateFallbackExceptionDetails)
                        .OfType<RunReportExceptionDetails>()
                        .ToArray()
                    : [],
            };
    }

    private async Task WriteReportAsync(string? reportPath, PipelineRunReport report)
    {
        if (reportPath is null)
        {
            return;
        }

        try
        {
            var fullPath = Path.GetFullPath(reportPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            await File.WriteAllTextAsync(fullPath, RunReportJsonSerializer.Serialize(report))
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not write pipeline run report to {RunReportPath}", reportPath);
        }
    }

    private async Task SaveHistoryAsync(bool historyEnabled, PipelineRunReport report)
    {
        if (!historyEnabled)
        {
            return;
        }

        try
        {
            using var timeout = new CancellationTokenSource(HistoryStoreTimeout);
            await historyStore.SaveAsync(report, timeout.Token).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not save pipeline run history");
        }
    }

    internal string? GetReportPath()
    {
        var options = pipelineOptions.Value.RunReport;
        if (!string.IsNullOrWhiteSpace(options.ReportPath))
        {
            return options.ReportPath;
        }

        return options.AutoWriteInCi && buildSystemDetector.IsKnownBuildAgent
            ? Path.Combine("artifacts", "run-report.json")
            : null;
    }

    private bool IsDistributedWorker()
    {
        var options = distributedOptions.Value;
        return options.Enabled
               && options.TotalInstances > 1
               && roleDetector.DetectRole() == DistributedRole.Worker;
    }

    private bool IsDistributedMaster()
    {
        var options = distributedOptions.Value;
        return options.Enabled
               && options.TotalInstances > 1
               && roleDetector.DetectRole() == DistributedRole.Master;
    }

    private async Task PublishWorkerMetricsAsync()
    {
        var options = distributedOptions.Value;
        var capabilities = new HashSet<string>(options.Capabilities, StringComparer.OrdinalIgnoreCase);
        if (options.AutoDetectOsCapability)
        {
            capabilities.UnionWith(OsCapabilityDetector.Detect());
        }

        try
        {
            using var timeout = new CancellationTokenSource(WorkerMetricsTimeout);
            await distributedCoordinator.RegisterWorkerAsync(
                    new WorkerRegistration(
                        options.InstanceIndex,
                        capabilities,
                        DateTimeOffset.UtcNow)
                    {
                        ExecutionIdentifier = options.ExecutionIdentifier,
                        UnattributedCommandCount = commandExecutionCounter.UnattributedCount,
                        ModuleCommandCounts = commandExecutionCounter.GetModuleCounts()
                            .GroupBy(
                                static count => ModuleTypeIdentifier.Get(count.Key),
                                StringComparer.Ordinal)
                            .ToDictionary(
                                static group => group.Key,
                                static group => group.Sum(count => count.Value),
                                StringComparer.Ordinal),
                    },
                    timeout.Token)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not publish distributed worker command metrics");
        }
    }

    private async Task AggregateWorkerMetricsAsync(PipelineSummary summary)
    {
        using var timeout = new CancellationTokenSource(WorkerMetricsTimeout);
        try
        {
            var options = distributedOptions.Value;
            var waitResult = await WaitForFinalWorkerMetricsAsync(
                    options.InstanceIndex,
                    options.ExecutionIdentifier,
                    timeout.Token)
                .ConfigureAwait(false);
            if (!waitResult.Completed)
            {
                logger.LogWarning("Timed out waiting for distributed worker command metrics");
            }

            var workerRegistrations = waitResult.Registrations;
            var completedWorkers = workerRegistrations
                .Where(worker => worker.UnattributedCommandCount.HasValue)
                .ToArray();
            foreach (var worker in completedWorkers)
            {
                commandExecutionCounter.Add(null, worker.UnattributedCommandCount.GetValueOrDefault());
            }

            ReconcileWorkerModuleCommandCounts(summary, completedWorkers);

            var incompleteWorkerCount = Math.Max(
                0,
                waitResult.ParticipantCount - completedWorkers.Length);
            if (incompleteWorkerCount > 0)
            {
                logger.LogWarning(
                    "Ignored {WorkerCount} distributed worker registrations without final command metrics",
                    incompleteWorkerCount);
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not collect distributed worker command metrics");
        }
    }

    private void ReconcileWorkerModuleCommandCounts(
        PipelineSummary summary,
        IReadOnlyCollection<WorkerRegistration> completedWorkers)
    {
        var moduleTypesByIdentifier = summary.Modules
            .Select(static module => module.GetType())
            .Distinct()
            .GroupBy(ModuleTypeIdentifier.Get, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.ToArray(),
                StringComparer.Ordinal);
        var finalCounts = completedWorkers
            .Where(static worker => worker.ModuleCommandCounts is not null)
            .SelectMany(static worker => worker.ModuleCommandCounts!)
            .GroupBy(static count => count.Key, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.Sum(count => count.Value),
                StringComparer.Ordinal);

        foreach (var (moduleTypeIdentifier, finalCount) in finalCounts)
        {
            if (!moduleTypesByIdentifier.TryGetValue(moduleTypeIdentifier, out var moduleTypes))
            {
                commandExecutionCounter.Add(null, finalCount);
                continue;
            }

            var recordedCount = moduleTypes.Sum(commandExecutionCounter.GetCount);
            var missingCount = finalCount - recordedCount;
            if (moduleTypes.Length == 1)
            {
                commandExecutionCounter.Add(moduleTypes[0], missingCount);
            }
            else
            {
                commandExecutionCounter.Add(null, missingCount);
            }
        }
    }

    private async Task<WorkerMetricsWaitResult> WaitForFinalWorkerMetricsAsync(
        int masterInstanceIndex,
        string? executionIdentifier,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkerRegistration> initialWorkers;
        try
        {
            initialWorkers = await distributedCoordinator
                .GetRegisteredWorkersAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return new WorkerMetricsWaitResult([], ParticipantCount: 0, Completed: false);
        }

        var expectedWorkerIndexes = initialWorkers
            .Where(worker => worker.WorkerIndex != masterInstanceIndex
                             && IsCurrentExecution(worker, executionIdentifier))
            .Select(worker => worker.WorkerIndex)
            .ToHashSet();
        var workerRegistrations = GetLatestWorkerRegistrations(
            initialWorkers,
            expectedWorkerIndexes,
            executionIdentifier);

        while (!cancellationToken.IsCancellationRequested)
        {
            if (expectedWorkerIndexes.All(workerIndex =>
                    workerRegistrations.Any(worker =>
                        worker.WorkerIndex == workerIndex
                        && worker.UnattributedCommandCount.HasValue)))
            {
                return new WorkerMetricsWaitResult(
                    workerRegistrations,
                    expectedWorkerIndexes.Count,
                    Completed: true);
            }

            try
            {
                await Task.Delay(WorkerMetricsPollingInterval, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            IReadOnlyList<WorkerRegistration> workers;
            try
            {
                workers = await distributedCoordinator
                    .GetRegisteredWorkersAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            workerRegistrations = GetLatestWorkerRegistrations(
                workers,
                expectedWorkerIndexes,
                executionIdentifier);
        }

        return new WorkerMetricsWaitResult(
            workerRegistrations,
            expectedWorkerIndexes.Count,
            Completed: false);
    }

    private static WorkerRegistration[] GetLatestWorkerRegistrations(
        IEnumerable<WorkerRegistration> workers,
        HashSet<int> expectedWorkerIndexes,
        string? executionIdentifier) =>
        [.. workers
            .Where(worker => expectedWorkerIndexes.Contains(worker.WorkerIndex)
                             && IsCurrentExecution(worker, executionIdentifier))
            .GroupBy(worker => worker.WorkerIndex)
            .Select(group => group.MaxBy(worker => worker.RegisteredAt)!)];

    private static bool IsCurrentExecution(
        WorkerRegistration worker,
        string? executionIdentifier) =>
        string.IsNullOrWhiteSpace(executionIdentifier)
        || string.Equals(
            worker.ExecutionIdentifier,
            executionIdentifier,
            StringComparison.Ordinal);

    private string GetPipelineIdentity(PipelineSummary summary, string? reportPath)
    {
        var configuredIdentity = pipelineOptions.Value.RunReport.PipelineIdentity;
        if (!string.IsNullOrWhiteSpace(configuredIdentity))
        {
            return configuredIdentity;
        }

        var definition = string.Join(
            '\n',
            new[] { reportPath?.Replace('\\', '/') ?? string.Empty }
                .Concat(summary.Modules
                    .Select(static module => ModuleTypeIdentifier.Get(module.GetType()))
                    .OrderBy(static name => name, StringComparer.Ordinal)));
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(definition)))
            .ToLowerInvariant();
    }

    private sealed record WorkerMetricsWaitResult(
        WorkerRegistration[] Registrations,
        int ParticipantCount,
        bool Completed);
}
