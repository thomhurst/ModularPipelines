using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-mover", "move-collection", "create")]
public record AzResourceMoverMoveCollectionCreateOptions(
[property: CommandSwitch("--move-collection-name")] string MoveCollectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--move-region")]
    public string? MoveRegion { get; set; }

    [CommandSwitch("--move-type")]
    public string? MoveType { get; set; }

    [CommandSwitch("--source-region")]
    public string? SourceRegion { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-region")]
    public string? TargetRegion { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}