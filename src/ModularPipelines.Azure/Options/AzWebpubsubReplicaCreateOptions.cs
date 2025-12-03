using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webpubsub", "replica", "create")]
public record AzWebpubsubReplicaCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--replica-name")] string ReplicaName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--unit-count")]
    public int? UnitCount { get; set; }
}