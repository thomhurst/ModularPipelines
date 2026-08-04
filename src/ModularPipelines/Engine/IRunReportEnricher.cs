namespace ModularPipelines.Engine;

/// <summary>
/// Adds environment-specific correlation metadata to a pipeline run report.
/// </summary>
public interface IRunReportEnricher
{
    /// <summary>
    /// Enriches the current run report metadata.
    /// </summary>
    /// <param name="context">The mutable enrichment context.</param>
    /// <param name="cancellationToken">A bounded token for enrichment work.</param>
    /// <returns>A value task representing the enrichment operation.</returns>
    ValueTask EnrichAsync(
        RunReportEnrichmentContext context,
        CancellationToken cancellationToken = default);
}
