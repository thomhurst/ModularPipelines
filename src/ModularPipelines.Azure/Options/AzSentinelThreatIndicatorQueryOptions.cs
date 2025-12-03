using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "threat-indicator", "query")]
public record AzSentinelThreatIndicatorQueryOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--include-disabled")]
    public bool? IncludeDisabled { get; set; }

    [CliOption("--keywords")]
    public string? Keywords { get; set; }

    [CliOption("--max-confidence")]
    public string? MaxConfidence { get; set; }

    [CliOption("--max-valid-until")]
    public string? MaxValidUntil { get; set; }

    [CliOption("--min-confidence")]
    public string? MinConfidence { get; set; }

    [CliOption("--min-valid-until")]
    public string? MinValidUntil { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--pattern-types")]
    public string? PatternTypes { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sources")]
    public string? Sources { get; set; }

    [CliOption("--threat-types")]
    public string? ThreatTypes { get; set; }
}