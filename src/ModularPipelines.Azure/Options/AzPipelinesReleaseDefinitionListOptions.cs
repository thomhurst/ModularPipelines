using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "release", "definition", "list")]
public record AzPipelinesReleaseDefinitionListOptions : AzOptions
{
    [CliOption("--artifact-source-id")]
    public string? ArtifactSourceId { get; set; }

    [CliOption("--artifact-type")]
    public string? ArtifactType { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}