namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for the module scheduler
/// </summary>
public class SchedulerOptions
{
    /// <summary>
    /// Timeout for waiting on scheduler notifications before re-checking state.
    /// This periodic re-check ensures we don't miss signals due to race conditions.
    /// Default: 100ms
    /// </summary>
    public TimeSpan NotificationTimeout { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Whether to enable detailed diagnostic logging for scheduler operations.
    /// This includes pending module tracking, constraint violations, and state transitions.
    /// Default: false
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = false;

    /// <summary>
    /// Whether to collect and log timing metrics for module queue times and execution.
    /// Default: true
    /// </summary>
    public bool EnableTimingMetrics { get; set; } = true;
}
