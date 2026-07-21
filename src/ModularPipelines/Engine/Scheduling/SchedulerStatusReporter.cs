using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Scheduling;

/// <summary>
/// Reports scheduler status changes at regular check intervals for diagnostic purposes.
/// </summary>
/// <remarks>
/// This class encapsulates the periodic status logging responsibility
/// previously embedded in ModuleScheduler, following the Single Responsibility Principle.
/// The status reporting helps diagnose pipeline progress by logging:
/// - Overall statistics (total, queued, executing, completed, pending modules)
/// - Pending modules with their unresolved dependency counts
/// - Currently executing modules
/// </remarks>
internal class SchedulerStatusReporter : ISchedulerStatusReporter
{
    private static readonly TimeSpan StatusCheckInterval = TimeSpan.FromSeconds(15);

    private readonly ILogger<SchedulerStatusReporter> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly object _statusLock = new();

    private DateTimeOffset _lastStatusCheckTime;
    private SchedulerStatusSnapshot? _lastSnapshot;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulerStatusReporter"/> class.
    /// </summary>
    /// <param name="logger">Logger for status output.</param>
    /// <param name="timeProvider">Provider for current time.</param>
    public SchedulerStatusReporter(ILogger<SchedulerStatusReporter> logger, TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc />
    public void LogStatusIfIntervalElapsed(ModuleStateQueries stateQueries, ReaderWriterLockSlim stateLock)
    {
        var now = _timeProvider.GetUtcNow();
        lock (_statusLock)
        {
            if (now - _lastStatusCheckTime < StatusCheckInterval)
            {
                return;
            }

            _lastStatusCheckTime = now;
        }

        // Consolidate all state queries under a single read lock to reduce contention
        SchedulerStatusSnapshot snapshot;

        stateLock.EnterReadLock();
        try
        {
            snapshot = new SchedulerStatusSnapshot(
                stateQueries.GetStatistics(),
                string.Join(", ", stateQueries.GetPendingModules()
                    .Select(FormatModuleWithDependencyCount)
                    .Order(StringComparer.Ordinal)),
                string.Join(", ", stateQueries.GetExecutingModules()
                    .Select(m => m.ModuleType.Name)
                    .Order(StringComparer.Ordinal)));
        }
        finally
        {
            stateLock.ExitReadLock();
        }

        lock (_statusLock)
        {
            if (snapshot == _lastSnapshot)
            {
                return;
            }

            _lastSnapshot = snapshot;
        }

        // All logging outside lock to avoid holding lock during I/O
        _logger.LogDebug(
            "Scheduler waiting: Total={Total}, Queued={Queued}, Executing={Executing}, Completed={Completed}, Pending={Pending}",
            snapshot.Statistics.Total,
            snapshot.Statistics.Queued,
            snapshot.Statistics.Executing,
            snapshot.Statistics.Completed,
            snapshot.Statistics.Pending);

        if (snapshot.PendingModules.Length > 0)
        {
            _logger.LogDebug("Pending modules: {Modules}", snapshot.PendingModules);
        }

        if (snapshot.ExecutingModules.Length > 0)
        {
            _logger.LogDebug("Executing modules: {Modules}", snapshot.ExecutingModules);
        }
    }

    private static string FormatModuleWithDependencyCount(ModuleState m)
    {
        return $"{m.ModuleType.Name} (deps: {m.UnresolvedDependencies.Count})";
    }

    private sealed record SchedulerStatusSnapshot(
        ModuleStateStatistics Statistics,
        string PendingModules,
        string ExecutingModules);
}
