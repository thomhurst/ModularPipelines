using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-mover", "move-collection", "list-unresolved-dependency")]
public record AzResourceMoverMoveCollectionListUnresolvedDependencyOptions(
[property: CommandSwitch("--move-collection-name")] string MoveCollectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--dependency-level")]
    public string? DependencyLevel { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }
}