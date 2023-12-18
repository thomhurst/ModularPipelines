using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable-group", "variable", "update")]
public record AzPipelinesVariableGroupVariableUpdateOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--new-name")]
    public string? NewName { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--prompt-value")]
    public bool? PromptValue { get; set; }

    [BooleanCommandSwitch("--secret")]
    public bool? Secret { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}