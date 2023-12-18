using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "restore", "restore-disks")]
public record AzBackupRestoreRestoreDisksOptions(
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--disk-encryption-set-id")]
    public string? DiskEncryptionSetId { get; set; }

    [CommandSwitch("--diskslist")]
    public string? Diskslist { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--item-name")]
    public string? ItemName { get; set; }

    [CommandSwitch("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CommandSwitch("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--restore-as-unmanaged-disks")]
    public bool? RestoreAsUnmanagedDisks { get; set; }

    [CommandSwitch("--restore-mode")]
    public string? RestoreMode { get; set; }

    [BooleanCommandSwitch("--restore-only-osdisk")]
    public bool? RestoreOnlyOsdisk { get; set; }

    [BooleanCommandSwitch("--restore-to-staging-storage-account")]
    public bool? RestoreToStagingStorageAccount { get; set; }

    [CommandSwitch("--rp-name")]
    public string? RpName { get; set; }

    [CommandSwitch("--storage-account-resource-group")]
    public int? StorageAccountResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CommandSwitch("--target-subnet-name")]
    public string? TargetSubnetName { get; set; }

    [CommandSwitch("--target-subscription-id")]
    public string? TargetSubscriptionId { get; set; }

    [CommandSwitch("--target-vm-name")]
    public string? TargetVmName { get; set; }

    [CommandSwitch("--target-vnet-name")]
    public string? TargetVnetName { get; set; }

    [CommandSwitch("--target-vnet-resource-group")]
    public string? TargetVnetResourceGroup { get; set; }

    [CommandSwitch("--target-zone")]
    public string? TargetZone { get; set; }

    [CommandSwitch("--use-secondary-region")]
    public string? UseSecondaryRegion { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}