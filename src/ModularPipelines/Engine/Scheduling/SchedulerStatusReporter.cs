using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Scheduling;

/// <summary>
/// Reports scheduler status at regular intervals for diagnostic purposes.
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
    private static readonly TimeSpan StatusLogInterval = TimeSpan.FromSeconds(15);

    private readonly ILogger<SchedulerStatusReporter> _logger;
    private readonly TimeProvider _timeProvider;

    private DateTimeOffset _lastStatusLogTime;

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
        if (now - _lastStatusLogTime < StatusLogInterval)
        {
            return;
        }

        _lastStatusLogTime = now;

        // Consolidate all state queries under a single read lock to reduce contention
        ModuleStateStatistics stats;
        ModuleState[] pending;
        ModuleState[] executing;

        stateLock.EnterReadLock();
        try
        {
            stats = stateQueries.GetStatistics();
            pending = stateQueries.GetPendingModules().ToArray();
            executing = stateQueries.GetExecutingModules().ToArray();
        }
        finally
        {
            stateLock.ExitReadLock();
        }

        // All logging outside lock to avoid holding lock during I/O
        _logger.LogDebug(
            "Scheduler waiting: Total={Total}, Queued={Queued}, Executing={Executing}, Completed={Completed}, Pending={Pending}",
            stats.Total, stats.Queued, stats.Executing, stats.Completed, stats.Pending);

        if (pending.Length > 0)
        {
            _logger.LogDebug("Pending modules: {Modules}",
                string.Join(", ", pending.Select(FormatModuleWithDependencyCount)));
        }

        if (executing.Length > 0)
        {
            _logger.LogDebug("Executing modules: {Modules}",
                string.Join(", ", executing.Select(m => m.ModuleType.Name)));
        }
    }

    private static string FormatModuleWithDependencyCount(ModuleState m)
    {
        return $"{m.ModuleType.Name} (deps: {m.UnresolvedDependencies.Count})";
    }
}
