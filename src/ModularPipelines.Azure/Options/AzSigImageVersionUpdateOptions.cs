using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "image-version", "update")]
public record AzSigImageVersionUpdateOptions(
[property: CliOption("--gallery-image-definition")] string GalleryImageDefinition,
[property: CliOption("--gallery-image-version")] string GalleryImageVersion,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--allow-replicated-location-deletion")]
    public bool? AllowReplicatedLocationDeletion { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--target-edge-zones")]
    public string? TargetEdgeZones { get; set; }

    [CliOption("--target-regions")]
    public string? TargetRegions { get; set; }
}