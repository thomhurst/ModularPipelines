using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "create")]
public record AzVmCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CommandSwitch("--accept-term")]
    public string? AcceptTerm { get; set; }

    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [CommandSwitch("--asgs")]
    public string? Asgs { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [BooleanCommandSwitch("--attach-data-disks")]
    public bool? AttachDataDisks { get; set; }

    [BooleanCommandSwitch("--attach-os-disk")]
    public bool? AttachOsDisk { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--availability-set")]
    public string? AvailabilitySet { get; set; }

    [CommandSwitch("--boot-diagnostics-storage")]
    public string? BootDiagnosticsStorage { get; set; }

    [CommandSwitch("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CommandSwitch("--computer-name")]
    public string? ComputerName { get; set; }

    [CommandSwitch("--count")]
    public int? Count { get; set; }

    [CommandSwitch("--custom-data")]
    public string? CustomData { get; set; }

    [CommandSwitch("--data-disk-caching")]
    public string? DataDiskCaching { get; set; }

    [CommandSwitch("--data-disk-delete-option")]
    public string? DataDiskDeleteOption { get; set; }

    [CommandSwitch("--data-disk-encryption-sets")]
    public string? DataDiskEncryptionSets { get; set; }

    [CommandSwitch("--data-disk-sizes-gb")]
    public string? DataDiskSizesGb { get; set; }

    [BooleanCommandSwitch("--disable-integrity-monitoring-autoupgrade")]
    public bool? DisableIntegrityMonitoringAutoupgrade { get; set; }

    [CommandSwitch("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [BooleanCommandSwitch("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [BooleanCommandSwitch("--enable-auto-update")]
    public bool? EnableAutoUpdate { get; set; }

    [BooleanCommandSwitch("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [BooleanCommandSwitch("--enable-hotpatching")]
    public bool? EnableHotpatching { get; set; }

    [BooleanCommandSwitch("--enable-integrity-monitoring")]
    public bool? EnableIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [BooleanCommandSwitch("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [BooleanCommandSwitch("--encryption-at-host")]
    public bool? EncryptionAtHost { get; set; }

    [BooleanCommandSwitch("--ephemeral-os-disk")]
    public bool? EphemeralOsDisk { get; set; }

    [CommandSwitch("--ephemeral-os-disk-placement")]
    public string? EphemeralOsDiskPlacement { get; set; }

    [CommandSwitch("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--host-group")]
    public string? HostGroup { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-price")]
    public string? MaxPrice { get; set; }

    [CommandSwitch("--nic-delete-option")]
    public string? NicDeleteOption { get; set; }

    [CommandSwitch("--nics")]
    public string? Nics { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nsg")]
    public string? Nsg { get; set; }

    [CommandSwitch("--nsg-rule")]
    public string? NsgRule { get; set; }

    [CommandSwitch("--os-disk-caching")]
    public string? OsDiskCaching { get; set; }

    [CommandSwitch("--os-disk-delete-option")]
    public string? OsDiskDeleteOption { get; set; }

    [CommandSwitch("--os-disk-encryption-set")]
    public string? OsDiskEncryptionSet { get; set; }

    [CommandSwitch("--os-disk-name")]
    public string? OsDiskName { get; set; }

    [CommandSwitch("--os-disk-secure-vm-disk-encryption-set")]
    public string? OsDiskSecureVmDiskEncryptionSet { get; set; }

    [CommandSwitch("--os-disk-security-encryption-type")]
    public string? OsDiskSecurityEncryptionType { get; set; }

    [CommandSwitch("--os-disk-size-gb")]
    public string? OsDiskSizeGb { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--patch-mode")]
    public string? PatchMode { get; set; }

    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-promotion-code")]
    public string? PlanPromotionCode { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--platform-fault-domain")]
    public string? PlatformFaultDomain { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CommandSwitch("--public-ip-address-dns-name")]
    public string? PublicIpAddressDnsName { get; set; }

    [CommandSwitch("--public-ip-sku")]
    public string? PublicIpSku { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--security-type")]
    public string? SecurityType { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [BooleanCommandSwitch("--specialized")]
    public bool? Specialized { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-name")]
    public string? SshKeyName { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--storage-container-name")]
    public string? StorageContainerName { get; set; }

    [CommandSwitch("--storage-sku")]
    public string? StorageSku { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [BooleanCommandSwitch("--use-unmanaged-disk")]
    public bool? UseUnmanagedDisk { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CommandSwitch("--v-cpus-per-core")]
    public string? VCpusPerCore { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandSwitch("--vmss")]
    public string? Vmss { get; set; }

    [CommandSwitch("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}