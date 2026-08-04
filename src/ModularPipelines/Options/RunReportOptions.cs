namespace ModularPipelines.Options;

/// <summary>
/// Configures machine-readable pipeline run reports and local run history.
/// </summary>
public sealed record RunReportOptions
{
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
    /// The limit applies independently to each pipeline identity. History is retained independently
    /// of JSON report writing. Set to zero to disable it.
    /// </summary>
    public int HistoryRetention { get; init; } = 20;

    /// <summary>
    /// Gets the maximum total number of reports retained by the default history store across all
    /// pipeline identities. Set to zero to disable the global limit.
    /// </summary>
    public int GlobalHistoryRetention { get; init; } = 200;
}
