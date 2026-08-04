namespace ModularPipelines.Models;

/// <summary>
/// Describes an exception in a machine-readable pipeline run report.
/// </summary>
public sealed record RunReportExceptionDetails
{
    /// <summary>
    /// Gets the exception type name.
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Gets the exception message.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Gets the exception stack trace, when available.
    /// </summary>
    public string? StackTrace { get; init; }

    /// <summary>
    /// Gets the inner exception, when available.
    /// </summary>
    public RunReportExceptionDetails? InnerException { get; init; }

    /// <summary>
    /// Gets every inner exception when this exception represents multiple failures.
    /// </summary>
    public IReadOnlyList<RunReportExceptionDetails> InnerExceptions { get; init; } = [];
}
