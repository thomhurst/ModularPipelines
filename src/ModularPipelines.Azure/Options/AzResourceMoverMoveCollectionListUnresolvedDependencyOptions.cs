using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-mover", "move-collection", "list-unresolved-dependency")]
public record AzResourceMoverMoveCollectionListUnresolvedDependencyOptions(
[property: CliOption("--move-collection-name")] string MoveCollectionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--dependency-level")]
    public string? DependencyLevel { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }
}