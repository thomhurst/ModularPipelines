namespace ModularPipelines.Logging;

/// <summary>
/// Defines the severity level for pipeline summary log entries.
/// </summary>
public enum SummaryLogLevel
{
    /// <summary>
    /// Informational message providing general details about the pipeline run.
    /// </summary>
    Information,

    /// <summary>
    /// Success message indicating a positive outcome or milestone.
    /// </summary>
    Success,

    /// <summary>
    /// Warning message indicating a potential issue that did not prevent completion.
    /// </summary>
    Warning,

    /// <summary>
    /// Error message indicating a failure or critical issue.
    /// </summary>
    Error,
}
