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
    private const int MaxModuleDetails = 10;
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
        SchedulerStatusSnapshot snapshot;

        lock (_statusLock)
        {
            if (now - _lastStatusCheckTime < StatusCheckInterval)
            {
                return;
            }

            _lastStatusCheckTime = now;

            if (!_logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            // Consolidate all state queries under a single read lock to reduce contention
            stateLock.EnterReadLock();
            try
            {
                var includeModuleDetails = _logger.IsEnabled(LogLevel.Trace);
                snapshot = new SchedulerStatusSnapshot(
                    stateQueries.GetStatistics(),
                    includeModuleDetails
                        ? FormatModuleDetails(stateQueries.GetPendingModules().Select(FormatModuleWithDependencyCount))
                        : ModuleDetails.Empty,
                    includeModuleDetails
                        ? FormatModuleDetails(stateQueries.GetExecutingModules().Select(m => FormatModuleType(m.ModuleType)))
                        : ModuleDetails.Empty);
            }
            finally
            {
                stateLock.ExitReadLock();
            }

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

        if (snapshot.PendingModules.Display.Length > 0)
        {
            _logger.LogTrace("Pending modules: {Modules}", snapshot.PendingModules.Display);
        }

        if (snapshot.ExecutingModules.Display.Length > 0)
        {
            _logger.LogTrace("Executing modules: {Modules}", snapshot.ExecutingModules.Display);
        }
    }

    private static ModuleDetails FormatModuleDetails(IEnumerable<string> modules)
    {
        var orderedModules = modules.Order(StringComparer.Ordinal).ToList();
        var display = string.Join(", ", orderedModules.Take(MaxModuleDetails));
        var omittedCount = orderedModules.Count - MaxModuleDetails;

        if (omittedCount > 0)
        {
            display = $"{display}, ... (+{omittedCount} more)";
        }

        // Full fingerprint preserves change detection even when changed modules are outside the displayed subset.
        return new ModuleDetails(string.Join('\n', orderedModules), display);
    }

    private static string FormatModuleWithDependencyCount(ModuleState m)
    {
        return $"{FormatModuleType(m.ModuleType)} (deps: {m.UnresolvedDependencies.Count})";
    }

    private static string FormatModuleType(Type moduleType) => moduleType.FullName ?? moduleType.Name;

    private sealed record SchedulerStatusSnapshot(
        ModuleStateStatistics Statistics,
        ModuleDetails PendingModules,
        ModuleDetails ExecutingModules);

    private sealed record ModuleDetails(string Fingerprint, string Display)
    {
        public static ModuleDetails Empty { get; } = new(string.Empty, string.Empty);
    }
}
