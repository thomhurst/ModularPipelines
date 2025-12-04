using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource-mover", "move-collection", "create")]
public record AzResourceMoverMoveCollectionCreateOptions(
[property: CliOption("--move-collection-name")] string MoveCollectionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--move-region")]
    public string? MoveRegion { get; set; }

    [CliOption("--move-type")]
    public string? MoveType { get; set; }

    [CliOption("--source-region")]
    public string? SourceRegion { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-region")]
    public string? TargetRegion { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}