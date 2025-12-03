using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "threat-indicator", "replace-tag")]
public record AzSentinelThreatIndicatorReplaceTagOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--confidence")]
    public string? Confidence { get; set; }

    [CliOption("--created")]
    public string? Created { get; set; }

    [CliOption("--created-by-ref")]
    public string? CreatedByRef { get; set; }

    [CliFlag("--defanged")]
    public bool? Defanged { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--external-id")]
    public string? ExternalId { get; set; }

    [CliOption("--external-references")]
    public string? ExternalReferences { get; set; }

    [CliOption("--external-updated-time")]
    public string? ExternalUpdatedTime { get; set; }

    [CliOption("--granular-markings")]
    public string? GranularMarkings { get; set; }

    [CliOption("--indicator-types")]
    public string? IndicatorTypes { get; set; }

    [CliOption("--intelligence-tags")]
    public string? IntelligenceTags { get; set; }

    [CliOption("--kill-chain-phases")]
    public string? KillChainPhases { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--last-updated-time")]
    public string? LastUpdatedTime { get; set; }

    [CliOption("--modified")]
    public string? Modified { get; set; }

    [CliOption("--object-marking-refs")]
    public string? ObjectMarkingRefs { get; set; }

    [CliOption("--parsed-pattern")]
    public string? ParsedPattern { get; set; }

    [CliOption("--pattern")]
    public string? Pattern { get; set; }

    [CliOption("--pattern-type")]
    public string? PatternType { get; set; }

    [CliOption("--pattern-version")]
    public string? PatternVersion { get; set; }

    [CliFlag("--revoked")]
    public bool? Revoked { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--threat-types")]
    public string? ThreatTypes { get; set; }

    [CliOption("--valid-from")]
    public string? ValidFrom { get; set; }

    [CliOption("--valid-until")]
    public string? ValidUntil { get; set; }
}