using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable", "create")]
public record AzPipelinesVariableCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--allow-override")]
    public bool? AllowOverride { get; set; }

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

    [BooleanCommandSwitch("--secret")]
    public bool? Secret { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}