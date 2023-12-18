using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version", "create")]
public record AzSigImageVersionCreateOptions(
[property: CommandSwitch("--gallery-image-definition")] string GalleryImageDefinition,
[property: CommandSwitch("--gallery-image-version")] string GalleryImageVersion,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-replicated-location-deletion")]
    public bool? AllowReplicatedLocationDeletion { get; set; }

    [CommandSwitch("--data-snapshot-luns")]
    public string? DataSnapshotLuns { get; set; }

    [CommandSwitch("--data-snapshots")]
    public string? DataSnapshots { get; set; }

    [CommandSwitch("--data-vhds-luns")]
    public string? DataVhdsLuns { get; set; }

    [CommandSwitch("--data-vhds-sa")]
    public string? DataVhdsSa { get; set; }

    [CommandSwitch("--data-vhds-uris")]
    public string? DataVhdsUris { get; set; }

    [CommandSwitch("--end-of-life-date")]
    public string? EndOfLifeDate { get; set; }

    [BooleanCommandSwitch("--exclude-from-latest")]
    public bool? ExcludeFromLatest { get; set; }

    [CommandSwitch("--image-version")]
    public string? ImageVersion { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-image")]
    public string? ManagedImage { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-snapshot")]
    public string? OsSnapshot { get; set; }

    [CommandSwitch("--os-vhd-storage-account")]
    public int? OsVhdStorageAccount { get; set; }

    [CommandSwitch("--os-vhd-uri")]
    public string? OsVhdUri { get; set; }

    [CommandSwitch("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CommandSwitch("--replication-mode")]
    public string? ReplicationMode { get; set; }

    [CommandSwitch("--storage-account-type")]
    public int? StorageAccountType { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-edge-zone-encryption")]
    public string? TargetEdgeZoneEncryption { get; set; }

    [CommandSwitch("--target-edge-zones")]
    public string? TargetEdgeZones { get; set; }

    [CommandSwitch("--target-region-cvm-encryption")]
    public string? TargetRegionCvmEncryption { get; set; }

    [CommandSwitch("--target-region-encryption")]
    public string? TargetRegionEncryption { get; set; }

    [CommandSwitch("--target-regions")]
    public string? TargetRegions { get; set; }

    [CommandSwitch("--virtual-machine")]
    public string? VirtualMachine { get; set; }
}

