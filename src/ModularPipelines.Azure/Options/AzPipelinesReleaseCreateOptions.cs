using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "release", "create")]
public record AzPipelinesReleaseCreateOptions : AzOptions
{
    [CommandSwitch("--artifact-metadata-list")]
    public string? ArtifactMetadataList { get; set; }

    [CommandSwitch("--definition-id")]
    public string? DefinitionId { get; set; }

    [CommandSwitch("--definition-name")]
    public string? DefinitionName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--open")]
    public bool? Open { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}