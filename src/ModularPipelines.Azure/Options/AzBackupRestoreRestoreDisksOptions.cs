using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "restore", "restore-disks")]
public record AzBackupRestoreRestoreDisksOptions(
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--disk-encryption-set-id")]
    public string? DiskEncryptionSetId { get; set; }

    [CliOption("--diskslist")]
    public string? Diskslist { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--item-name")]
    public string? ItemName { get; set; }

    [CliOption("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CliOption("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--restore-as-unmanaged-disks")]
    public bool? RestoreAsUnmanagedDisks { get; set; }

    [CliOption("--restore-mode")]
    public string? RestoreMode { get; set; }

    [CliFlag("--restore-only-osdisk")]
    public bool? RestoreOnlyOsdisk { get; set; }

    [CliFlag("--restore-to-staging-storage-account")]
    public bool? RestoreToStagingStorageAccount { get; set; }

    [CliOption("--rp-name")]
    public string? RpName { get; set; }

    [CliOption("--storage-account-resource-group")]
    public int? StorageAccountResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CliOption("--target-subnet-name")]
    public string? TargetSubnetName { get; set; }

    [CliOption("--target-subscription-id")]
    public string? TargetSubscriptionId { get; set; }

    [CliOption("--target-vm-name")]
    public string? TargetVmName { get; set; }

    [CliOption("--target-vnet-name")]
    public string? TargetVnetName { get; set; }

    [CliOption("--target-vnet-resource-group")]
    public string? TargetVnetResourceGroup { get; set; }

    [CliOption("--target-zone")]
    public string? TargetZone { get; set; }

    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}