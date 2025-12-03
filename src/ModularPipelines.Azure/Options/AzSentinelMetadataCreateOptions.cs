using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "metadata", "create")]
public record AzSentinelMetadataCreateOptions(
[property: CliOption("--metadata-name")] string MetadataName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--author")]
    public string? Author { get; set; }

    [CliOption("--categories")]
    public string? Categories { get; set; }

    [CliOption("--content-id")]
    public string? ContentId { get; set; }

    [CliOption("--content-schema-version")]
    public string? ContentSchemaVersion { get; set; }

    [CliOption("--custom-version")]
    public string? CustomVersion { get; set; }

    [CliOption("--dependencies")]
    public string? Dependencies { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--first-publish-date")]
    public string? FirstPublishDate { get; set; }

    [CliOption("--icon")]
    public string? Icon { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--last-publish-date")]
    public string? LastPublishDate { get; set; }

    [CliOption("--parent-id")]
    public string? ParentId { get; set; }

    [CliOption("--preview-images")]
    public string? PreviewImages { get; set; }

    [CliOption("--preview-images-dark")]
    public string? PreviewImagesDark { get; set; }

    [CliOption("--providers")]
    public string? Providers { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--support")]
    public string? Support { get; set; }

    [CliOption("--threat-tactics")]
    public string? ThreatTactics { get; set; }

    [CliOption("--threat-techniques")]
    public string? ThreatTechniques { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}