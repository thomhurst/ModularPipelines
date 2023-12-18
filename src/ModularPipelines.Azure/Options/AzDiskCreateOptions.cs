using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk", "create")]
public record AzDiskCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--data-access-auth-mode")]
    public string? DataAccessAuthMode { get; set; }

    [CommandSwitch("--disk-access")]
    public string? DiskAccess { get; set; }

    [CommandSwitch("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CommandSwitch("--disk-iops-read-only")]
    public string? DiskIopsReadOnly { get; set; }

    [CommandSwitch("--disk-iops-read-write")]
    public string? DiskIopsReadWrite { get; set; }

    [CommandSwitch("--disk-mbps-read-only")]
    public string? DiskMbpsReadOnly { get; set; }

    [CommandSwitch("--disk-mbps-read-write")]
    public string? DiskMbpsReadWrite { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [BooleanCommandSwitch("--enable-bursting")]
    public bool? EnableBursting { get; set; }

    [CommandSwitch("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CommandSwitch("--gallery-image-reference")]
    public string? GalleryImageReference { get; set; }

    [CommandSwitch("--gallery-image-reference-lun")]
    public string? GalleryImageReferenceLun { get; set; }

    [CommandSwitch("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CommandSwitch("--image-reference")]
    public string? ImageReference { get; set; }

    [CommandSwitch("--image-reference-lun")]
    public string? ImageReferenceLun { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logical-sector-size")]
    public string? LogicalSectorSize { get; set; }

    [CommandSwitch("--max-shares")]
    public string? MaxShares { get; set; }

    [CommandSwitch("--network-access-policy")]
    public string? NetworkAccessPolicy { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--optimized-for-frequent-attach")]
    public bool? OptimizedForFrequentAttach { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [BooleanCommandSwitch("--performance-plus")]
    public bool? PerformancePlus { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--secure-vm-disk-encryption-set")]
    public string? SecureVmDiskEncryptionSet { get; set; }

    [CommandSwitch("--security-data-uri")]
    public string? SecurityDataUri { get; set; }

    [CommandSwitch("--security-type")]
    public string? SecurityType { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-storage-account-id")]
    public int? SourceStorageAccountId { get; set; }

    [BooleanCommandSwitch("--support-hibernation")]
    public bool? SupportHibernation { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--upload-size-bytes")]
    public string? UploadSizeBytes { get; set; }

    [CommandSwitch("--upload-type")]
    public string? UploadType { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}