using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "work-item", "relation", "add")]
public record AzBoardsWorkItemRelationAddOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--relation-type")] string RelationType
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }

    [CommandSwitch("--target-url")]
    public string? TargetUrl { get; set; }
}