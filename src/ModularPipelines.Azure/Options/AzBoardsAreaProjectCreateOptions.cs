using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "area", "project", "create")]
public record AzBoardsAreaProjectCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}