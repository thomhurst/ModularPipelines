using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "iteration", "project", "update")]
public record AzBoardsIterationProjectUpdateOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--child-id")]
    public string? ChildId { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--finish-date")]
    public string? FinishDate { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }
}

