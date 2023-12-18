using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable", "list")]
public record AzPipelinesVariableListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CommandSwitch("--pipeline-name")]
    public string? PipelineName { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}

