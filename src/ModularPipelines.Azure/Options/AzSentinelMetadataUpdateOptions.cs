using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "metadata", "update")]
public record AzSentinelMetadataUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--author")]
    public string? Author { get; set; }

    [CommandSwitch("--categories")]
    public string? Categories { get; set; }

    [CommandSwitch("--content-id")]
    public string? ContentId { get; set; }

    [CommandSwitch("--content-schema-version")]
    public string? ContentSchemaVersion { get; set; }

    [CommandSwitch("--custom-version")]
    public string? CustomVersion { get; set; }

    [CommandSwitch("--dependencies")]
    public string? Dependencies { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--first-publish-date")]
    public string? FirstPublishDate { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--icon")]
    public string? Icon { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--last-publish-date")]
    public string? LastPublishDate { get; set; }

    [CommandSwitch("--metadata-name")]
    public string? MetadataName { get; set; }

    [CommandSwitch("--parent-id")]
    public string? ParentId { get; set; }

    [CommandSwitch("--preview-images")]
    public string? PreviewImages { get; set; }

    [CommandSwitch("--preview-images-dark")]
    public string? PreviewImagesDark { get; set; }

    [CommandSwitch("--providers")]
    public string? Providers { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--support")]
    public string? Support { get; set; }

    [CommandSwitch("--threat-tactics")]
    public string? ThreatTactics { get; set; }

    [CommandSwitch("--threat-techniques")]
    public string? ThreatTechniques { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}