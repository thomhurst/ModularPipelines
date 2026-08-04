namespace ModularPipelines.Options;

/// <summary>
/// Configures machine-readable pipeline run reports and local run history.
/// </summary>
public sealed record RunReportOptions
{
    /// <summary>
    /// Gets an optional stable identity used to partition retained history.
    /// When omitted, an identity is derived from the registered module types.
    /// </summary>
    public string? PipelineIdentity { get; init; }

    /// <summary>
    /// Gets the explicit JSON report path, or <see langword="null"/> to use the CI default only.
    /// Relative paths are resolved from the Git root when available, otherwise the application base directory.
    /// </summary>
    public string? ReportPath { get; init; }

    /// <summary>
    /// Gets a value indicating whether known CI systems write <c>artifacts/run-report.json</c>
    /// when <see cref="ReportPath"/> is not configured.
    /// </summary>
    public bool AutoWriteInCi { get; init; } = true;

    /// <summary>
    /// Gets the directory used by the default local history store.
    /// Relative paths are resolved from the Git root when available, otherwise the application base directory.
    /// </summary>
    public string HistoryDirectory { get; init; } = Path.Combine(".modularpipelines", "run-history");

    /// <summary>
    /// Gets the maximum number of reports retained by the default history store.
    /// History is retained independently of JSON report writing. Set to zero to disable it.
    /// </summary>
    public int HistoryRetention { get; init; } = 20;
}
