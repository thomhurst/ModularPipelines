using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-mover", "move-resource", "add")]
public record AzResourceMoverMoveResourceAddOptions : AzOptions
{
    [CliOption("--depends-on-overrides")]
    public string? DependsOnOverrides { get; set; }

    [CliOption("--existing-target-id")]
    public string? ExistingTargetId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--move-collection-name")]
    public string? MoveCollectionName { get; set; }

    [CliOption("--move-resource-name")]
    public string? MoveResourceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-settings")]
    public string? ResourceSettings { get; set; }

    [CliOption("--source-id")]
    public string? SourceId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}