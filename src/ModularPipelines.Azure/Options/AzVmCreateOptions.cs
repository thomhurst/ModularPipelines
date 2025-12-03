using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "create")]
public record AzVmCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CliOption("--accept-term")]
    public string? AcceptTerm { get; set; }

    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--asgs")]
    public string? Asgs { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliFlag("--attach-data-disks")]
    public bool? AttachDataDisks { get; set; }

    [CliFlag("--attach-os-disk")]
    public bool? AttachOsDisk { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--availability-set")]
    public string? AvailabilitySet { get; set; }

    [CliOption("--boot-diagnostics-storage")]
    public string? BootDiagnosticsStorage { get; set; }

    [CliOption("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CliOption("--computer-name")]
    public string? ComputerName { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliOption("--custom-data")]
    public string? CustomData { get; set; }

    [CliOption("--data-disk-caching")]
    public string? DataDiskCaching { get; set; }

    [CliOption("--data-disk-delete-option")]
    public string? DataDiskDeleteOption { get; set; }

    [CliOption("--data-disk-encryption-sets")]
    public string? DataDiskEncryptionSets { get; set; }

    [CliOption("--data-disk-sizes-gb")]
    public string? DataDiskSizesGb { get; set; }

    [CliFlag("--disable-integrity-monitoring-autoupgrade")]
    public bool? DisableIntegrityMonitoringAutoupgrade { get; set; }

    [CliOption("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliFlag("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [CliFlag("--enable-auto-update")]
    public bool? EnableAutoUpdate { get; set; }

    [CliFlag("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [CliFlag("--enable-hotpatching")]
    public bool? EnableHotpatching { get; set; }

    [CliFlag("--enable-integrity-monitoring")]
    public bool? EnableIntegrityMonitoring { get; set; }

    [CliFlag("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [CliFlag("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CliFlag("--encryption-at-host")]
    public bool? EncryptionAtHost { get; set; }

    [CliFlag("--ephemeral-os-disk")]
    public bool? EphemeralOsDisk { get; set; }

    [CliOption("--ephemeral-os-disk-placement")]
    public string? EphemeralOsDiskPlacement { get; set; }

    [CliOption("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--host-group")]
    public string? HostGroup { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-price")]
    public string? MaxPrice { get; set; }

    [CliOption("--nic-delete-option")]
    public string? NicDeleteOption { get; set; }

    [CliOption("--nics")]
    public string? Nics { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nsg")]
    public string? Nsg { get; set; }

    [CliOption("--nsg-rule")]
    public string? NsgRule { get; set; }

    [CliOption("--os-disk-caching")]
    public string? OsDiskCaching { get; set; }

    [CliOption("--os-disk-delete-option")]
    public string? OsDiskDeleteOption { get; set; }

    [CliOption("--os-disk-encryption-set")]
    public string? OsDiskEncryptionSet { get; set; }

    [CliOption("--os-disk-name")]
    public string? OsDiskName { get; set; }

    [CliOption("--os-disk-secure-vm-disk-encryption-set")]
    public string? OsDiskSecureVmDiskEncryptionSet { get; set; }

    [CliOption("--os-disk-security-encryption-type")]
    public string? OsDiskSecurityEncryptionType { get; set; }

    [CliOption("--os-disk-size-gb")]
    public string? OsDiskSizeGb { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--patch-mode")]
    public string? PatchMode { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-promotion-code")]
    public string? PlanPromotionCode { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--platform-fault-domain")]
    public string? PlatformFaultDomain { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CliOption("--public-ip-address-dns-name")]
    public string? PublicIpAddressDnsName { get; set; }

    [CliOption("--public-ip-sku")]
    public string? PublicIpSku { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--security-type")]
    public string? SecurityType { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliFlag("--specialized")]
    public bool? Specialized { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-name")]
    public string? SshKeyName { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--storage-container-name")]
    public string? StorageContainerName { get; set; }

    [CliOption("--storage-sku")]
    public string? StorageSku { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CliFlag("--use-unmanaged-disk")]
    public bool? UseUnmanagedDisk { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CliOption("--v-cpus-per-core")]
    public string? VCpusPerCore { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }

    [CliOption("--vmss")]
    public string? Vmss { get; set; }

    [CliOption("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}