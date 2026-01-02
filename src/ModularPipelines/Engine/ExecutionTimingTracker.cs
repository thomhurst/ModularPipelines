using System.Diagnostics;

namespace ModularPipelines.Engine;

/// <summary>
/// Tracks execution timing for modules and sub-modules.
/// Provides a unified interface for timing operations to avoid code duplication.
/// </summary>
/// <remarks>
/// This class consolidates timing logic that was previously duplicated in:
/// - <see cref="ModuleExecutionContext"/> for module execution
/// - SubModule for sub-module execution
///
/// Usage:
/// <code>
/// var tracker = new ExecutionTimingTracker();
/// tracker.Start();
/// // ... execute work ...
/// tracker.Stop();
/// // Access tracker.StartTime, tracker.EndTime, tracker.Duration
/// </code>
/// </remarks>
internal sealed class ExecutionTimingTracker
{
    private readonly Stopwatch _stopwatch = new();

    /// <summary>
    /// Gets the time when tracking started.
    /// </summary>
    public DateTimeOffset StartTime { get; private set; }

    /// <summary>
    /// Gets the time when tracking stopped.
    /// </summary>
    public DateTimeOffset EndTime { get; private set; }

    /// <summary>
    /// Gets the elapsed duration.
    /// </summary>
    public TimeSpan Duration { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the tracker is currently running.
    /// </summary>
    public bool IsRunning => _stopwatch.IsRunning;

    /// <summary>
    /// Starts tracking execution time.
    /// </summary>
    public void Start()
    {
        StartTime = DateTimeOffset.UtcNow;
        _stopwatch.Start();
    }

    /// <summary>
    /// Stops tracking and records final timing values.
    /// </summary>
    public void Stop()
    {
        _stopwatch.Stop();
        EndTime = DateTimeOffset.UtcNow;
        Duration = _stopwatch.Elapsed;
    }

    /// <summary>
    /// Gets the current elapsed time without stopping the tracker.
    /// </summary>
    public TimeSpan GetElapsed() => _stopwatch.Elapsed;
}
