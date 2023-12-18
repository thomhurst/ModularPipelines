using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-mover", "move-resource", "add")]
public record AzResourceMoverMoveResourceAddOptions(
[property: CommandSwitch("--move-collection-name")] string MoveCollectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--depends-on-overrides")]
    public string? DependsOnOverrides { get; set; }

    [CommandSwitch("--existing-target-id")]
    public string? ExistingTargetId { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--move-resource-name")]
    public string? MoveResourceName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-settings")]
    public string? ResourceSettings { get; set; }

    [CommandSwitch("--source-id")]
    public string? SourceId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}