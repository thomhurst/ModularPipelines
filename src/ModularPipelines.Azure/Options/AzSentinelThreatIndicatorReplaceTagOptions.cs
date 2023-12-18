using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "threat-indicator", "replace-tag")]
public record AzSentinelThreatIndicatorReplaceTagOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--confidence")]
    public string? Confidence { get; set; }

    [CommandSwitch("--created")]
    public string? Created { get; set; }

    [CommandSwitch("--created-by-ref")]
    public string? CreatedByRef { get; set; }

    [BooleanCommandSwitch("--defanged")]
    public bool? Defanged { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--external-id")]
    public string? ExternalId { get; set; }

    [CommandSwitch("--external-references")]
    public string? ExternalReferences { get; set; }

    [CommandSwitch("--external-updated-time")]
    public string? ExternalUpdatedTime { get; set; }

    [CommandSwitch("--granular-markings")]
    public string? GranularMarkings { get; set; }

    [CommandSwitch("--indicator-types")]
    public string? IndicatorTypes { get; set; }

    [CommandSwitch("--intelligence-tags")]
    public string? IntelligenceTags { get; set; }

    [CommandSwitch("--kill-chain-phases")]
    public string? KillChainPhases { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--last-updated-time")]
    public string? LastUpdatedTime { get; set; }

    [CommandSwitch("--modified")]
    public string? Modified { get; set; }

    [CommandSwitch("--object-marking-refs")]
    public string? ObjectMarkingRefs { get; set; }

    [CommandSwitch("--parsed-pattern")]
    public string? ParsedPattern { get; set; }

    [CommandSwitch("--pattern")]
    public string? Pattern { get; set; }

    [CommandSwitch("--pattern-type")]
    public string? PatternType { get; set; }

    [CommandSwitch("--pattern-version")]
    public string? PatternVersion { get; set; }

    [BooleanCommandSwitch("--revoked")]
    public bool? Revoked { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--threat-types")]
    public string? ThreatTypes { get; set; }

    [CommandSwitch("--valid-from")]
    public string? ValidFrom { get; set; }

    [CommandSwitch("--valid-until")]
    public string? ValidUntil { get; set; }
}