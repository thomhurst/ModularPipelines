using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource-mover", "move-collection", "commit")]
public record AzResourceMoverMoveCollectionCommitOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--input-type")]
    public string? InputType { get; set; }

    [CliOption("--move-collection-name")]
    public string? MoveCollectionName { get; set; }

    [CliOption("--move-resources")]
    public string? MoveResources { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}