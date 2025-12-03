using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk", "create")]
public record AzDiskCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--data-access-auth-mode")]
    public string? DataAccessAuthMode { get; set; }

    [CliOption("--disk-access")]
    public string? DiskAccess { get; set; }

    [CliOption("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CliOption("--disk-iops-read-only")]
    public string? DiskIopsReadOnly { get; set; }

    [CliOption("--disk-iops-read-write")]
    public string? DiskIopsReadWrite { get; set; }

    [CliOption("--disk-mbps-read-only")]
    public string? DiskMbpsReadOnly { get; set; }

    [CliOption("--disk-mbps-read-write")]
    public string? DiskMbpsReadWrite { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliFlag("--enable-bursting")]
    public bool? EnableBursting { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliOption("--gallery-image-reference")]
    public string? GalleryImageReference { get; set; }

    [CliOption("--gallery-image-reference-lun")]
    public string? GalleryImageReferenceLun { get; set; }

    [CliOption("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CliOption("--image-reference")]
    public string? ImageReference { get; set; }

    [CliOption("--image-reference-lun")]
    public string? ImageReferenceLun { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logical-sector-size")]
    public string? LogicalSectorSize { get; set; }

    [CliOption("--max-shares")]
    public string? MaxShares { get; set; }

    [CliOption("--network-access-policy")]
    public string? NetworkAccessPolicy { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--optimized-for-frequent-attach")]
    public bool? OptimizedForFrequentAttach { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliFlag("--performance-plus")]
    public bool? PerformancePlus { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--secure-vm-disk-encryption-set")]
    public string? SecureVmDiskEncryptionSet { get; set; }

    [CliOption("--security-data-uri")]
    public string? SecurityDataUri { get; set; }

    [CliOption("--security-type")]
    public string? SecurityType { get; set; }

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--source-storage-account-id")]
    public int? SourceStorageAccountId { get; set; }

    [CliFlag("--support-hibernation")]
    public bool? SupportHibernation { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--upload-size-bytes")]
    public string? UploadSizeBytes { get; set; }

    [CliOption("--upload-type")]
    public string? UploadType { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}