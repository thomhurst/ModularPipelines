using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-version", "create")]
public record AzSigImageVersionCreateOptions(
[property: CliOption("--gallery-image-definition")] string GalleryImageDefinition,
[property: CliOption("--gallery-image-version")] string GalleryImageVersion,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-replicated-location-deletion")]
    public bool? AllowReplicatedLocationDeletion { get; set; }

    [CliOption("--data-snapshot-luns")]
    public string? DataSnapshotLuns { get; set; }

    [CliOption("--data-snapshots")]
    public string? DataSnapshots { get; set; }

    [CliOption("--data-vhds-luns")]
    public string? DataVhdsLuns { get; set; }

    [CliOption("--data-vhds-sa")]
    public string? DataVhdsSa { get; set; }

    [CliOption("--data-vhds-uris")]
    public string? DataVhdsUris { get; set; }

    [CliOption("--end-of-life-date")]
    public string? EndOfLifeDate { get; set; }

    [CliFlag("--exclude-from-latest")]
    public bool? ExcludeFromLatest { get; set; }

    [CliOption("--image-version")]
    public string? ImageVersion { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-image")]
    public string? ManagedImage { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-snapshot")]
    public string? OsSnapshot { get; set; }

    [CliOption("--os-vhd-storage-account")]
    public int? OsVhdStorageAccount { get; set; }

    [CliOption("--os-vhd-uri")]
    public string? OsVhdUri { get; set; }

    [CliOption("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CliOption("--replication-mode")]
    public string? ReplicationMode { get; set; }

    [CliOption("--storage-account-type")]
    public int? StorageAccountType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-edge-zone-encryption")]
    public string? TargetEdgeZoneEncryption { get; set; }

    [CliOption("--target-edge-zones")]
    public string? TargetEdgeZones { get; set; }

    [CliOption("--target-region-cvm-encryption")]
    public string? TargetRegionCvmEncryption { get; set; }

    [CliOption("--target-region-encryption")]
    public string? TargetRegionEncryption { get; set; }

    [CliOption("--target-regions")]
    public string? TargetRegions { get; set; }

    [CliOption("--virtual-machine")]
    public string? VirtualMachine { get; set; }
}