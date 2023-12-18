using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable-group", "variable", "delete")]
public record AzPipelinesVariableGroupVariableDeleteOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}