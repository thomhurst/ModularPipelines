using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable-group", "create")]
public record AzPipelinesVariableGroupCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--variables")] string Variables
) : AzOptions
{
    [BooleanCommandSwitch("--authorize")]
    public bool? Authorize { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}