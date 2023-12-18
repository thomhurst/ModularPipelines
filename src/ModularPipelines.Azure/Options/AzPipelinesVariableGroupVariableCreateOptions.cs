using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "variable-group", "variable", "create")]
public record AzPipelinesVariableGroupVariableCreateOptions(
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

    [BooleanCommandSwitch("--secret")]
    public bool? Secret { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}