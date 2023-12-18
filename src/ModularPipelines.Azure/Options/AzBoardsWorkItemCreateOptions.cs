using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "work-item", "create")]
public record AzBoardsWorkItemCreateOptions(
[property: CommandSwitch("--title")] string Title,
[property: CommandSwitch("--type")] string Type
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

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }
}