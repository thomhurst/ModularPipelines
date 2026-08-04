namespace ModularPipelines.Options;

/// <summary>
/// Configures machine-readable pipeline run reports and local run history.
/// </summary>
public sealed record RunReportOptions
{
    /// <summary>
    /// Gets a value indicating whether module output excerpts are included in reports and history.
    /// Output is secret-masked and retained from the tail within
    /// <see cref="MaxOutputBytesPerModule"/>.
    /// </summary>
    public bool IncludeModuleOutput { get; init; }

    /// <summary>
    /// Gets the shared UTF-8 byte limit for each module's standard-output and standard-error tails.
    /// </summary>
    public int MaxOutputBytesPerModule { get; init; } = 8 * 1024;

    /// <summary>
    /// Gets an optional stable identity used to partition retained history.
    /// When omitted, an identity is derived from the report path and module types.
    /// </summary>
    public string? PipelineIdentity { get; init; }

    /// <summary>
    /// Gets the explicit JSON report path, or <see langword="null"/> to use the CI default only.
    /// </summary>
    public string? ReportPath { get; init; }

    /// <summary>
    /// Gets a value indicating whether known CI systems write <c>artifacts/run-report.json</c>
    /// when <see cref="ReportPath"/> is not configured.
    /// </summary>
    public bool AutoWriteInCi { get; init; } = true;

    /// <summary>
    /// Gets the directory used by the default local history store.
    /// </summary>
    public string HistoryDirectory { get; init; } = Path.Combine(".modularpipelines", "run-history");

    /// <summary>
    /// Gets the maximum number of reports retained by the default history store.
    /// History is retained independently of JSON report writing. Set to zero to disable it.
    /// </summary>
    public int HistoryRetention { get; init; } = 20;
}
