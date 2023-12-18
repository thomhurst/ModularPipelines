using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "work-item", "update")]
public record AzBoardsWorkItemUpdateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--area")]
    public string? Area { get; set; }

    [CommandSwitch("--assigned-to")]
    public string? AssignedTo { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--discussion")]
    public string? Discussion { get; set; }

    [CommandSwitch("--fields")]
    public string? Fields { get; set; }

    [CommandSwitch("--iteration")]
    public string? Iteration { get; set; }

    [BooleanCommandSwitch("--open")]
    public bool? Open { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }
}