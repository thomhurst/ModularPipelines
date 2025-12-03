using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "release", "create")]
public record AzPipelinesReleaseCreateOptions : AzOptions
{
    [CliOption("--artifact-metadata-list")]
    public string? ArtifactMetadataList { get; set; }

    [CliOption("--definition-id")]
    public string? DefinitionId { get; set; }

    [CliOption("--definition-name")]
    public string? DefinitionName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}