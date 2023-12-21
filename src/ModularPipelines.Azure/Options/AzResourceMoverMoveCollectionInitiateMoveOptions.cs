using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-mover", "move-collection", "initiate-move")]
public record AzResourceMoverMoveCollectionInitiateMoveOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--input-type")]
    public string? InputType { get; set; }

    [CommandSwitch("--move-collection-name")]
    public string? MoveCollectionName { get; set; }

    [CommandSwitch("--move-resources")]
    public string? MoveResources { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}