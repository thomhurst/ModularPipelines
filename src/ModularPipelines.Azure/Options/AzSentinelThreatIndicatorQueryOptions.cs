using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "threat-indicator", "query")]
public record AzSentinelThreatIndicatorQueryOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--include-disabled")]
    public bool? IncludeDisabled { get; set; }

    [CommandSwitch("--keywords")]
    public string? Keywords { get; set; }

    [CommandSwitch("--max-confidence")]
    public string? MaxConfidence { get; set; }

    [CommandSwitch("--max-valid-until")]
    public string? MaxValidUntil { get; set; }

    [CommandSwitch("--min-confidence")]
    public string? MinConfidence { get; set; }

    [CommandSwitch("--min-valid-until")]
    public string? MinValidUntil { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--pattern-types")]
    public string? PatternTypes { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sources")]
    public string? Sources { get; set; }

    [CommandSwitch("--threat-types")]
    public string? ThreatTypes { get; set; }
}