using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable", "update")]
public record AzPipelinesVariableUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--allow-override")]
    public bool? AllowOverride { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--new-name")]
    public string? NewName { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CommandSwitch("--pipeline-name")]
    public string? PipelineName { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--prompt-value")]
    public bool? PromptValue { get; set; }

    [BooleanCommandSwitch("--secret")]
    public bool? Secret { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}