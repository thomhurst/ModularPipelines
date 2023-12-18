using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "release", "definition", "list")]
public record AzPipelinesReleaseDefinitionListOptions : AzOptions
{
    [CommandSwitch("--artifact-source-id")]
    public string? ArtifactSourceId { get; set; }

    [CommandSwitch("--artifact-type")]
    public string? ArtifactType { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}