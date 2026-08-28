using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for the module scheduler.
/// </summary>
[ExcludeFromCodeCoverage]
public record SchedulerOptions
{
    /// <summary>
    /// Gets or sets the confirmation delay before treating a blocked scheduler as deadlocked.
    /// Default: 100ms.
    /// </summary>
    public TimeSpan NotificationTimeout { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Gets or sets a value indicating whether to enable detailed diagnostic logging for scheduler operations.
    /// This includes pending module tracking, constraint violations, and state transitions.
    /// Default: false.
    /// </summary>
    internal bool EnableDetailedLogging { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to collect and log timing metrics for module queue times and execution.
    /// Default: true.
    /// </summary>
    internal bool EnableTimingMetrics { get; set; } = true;
}
