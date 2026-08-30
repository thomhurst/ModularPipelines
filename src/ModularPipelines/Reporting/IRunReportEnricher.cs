namespace ModularPipelines.Reporting;

/// <summary>
/// Adds environment-specific correlation metadata to a pipeline run report.
/// </summary>
public interface IRunReportEnricher
{
    /// <summary>
    /// Enriches the current run report metadata.
    /// </summary>
    /// <remarks>
    /// Enrichers run sequentially in registration order and receive metadata committed by earlier
    /// enrichers. Lower-confidence sources should fill missing values with <c>??=</c>. An
    /// authoritative source may overwrite an existing value deliberately; later authoritative
    /// enrichers take precedence.
    /// </remarks>
    /// <param name="context">The mutable enrichment context.</param>
    /// <param name="cancellationToken">A bounded token for enrichment work.</param>
    /// <returns>A value task representing the enrichment operation.</returns>
    ValueTask EnrichAsync(
        RunReportEnrichmentContext context,
        CancellationToken cancellationToken = default);
}
