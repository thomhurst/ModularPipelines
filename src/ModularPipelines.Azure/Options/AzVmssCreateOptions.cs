using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "create")]
public record AzVmssCreateOptions(
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

    [CliOption("--app-gateway")]
    public string? AppGateway { get; set; }

    [CliOption("--app-gateway-capacity")]
    public string? AppGatewayCapacity { get; set; }

    [CliOption("--app-gateway-sku")]
    public string? AppGatewaySku { get; set; }

    [CliOption("--app-gateway-subnet-address-prefix")]
    public string? AppGatewaySubnetAddressPrefix { get; set; }

    [CliOption("--asgs")]
    public string? Asgs { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--automatic-repairs-action")]
    public string? AutomaticRepairsAction { get; set; }

    [CliOption("--automatic-repairs-grace-period")]
    public string? AutomaticRepairsGracePeriod { get; set; }

    [CliOption("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CliOption("--backend-port")]
    public string? BackendPort { get; set; }

    [CliOption("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CliOption("--computer-name-prefix")]
    public string? ComputerNamePrefix { get; set; }

    [CliOption("--custom-data")]
    public string? CustomData { get; set; }

    [CliOption("--data-disk-caching")]
    public string? DataDiskCaching { get; set; }

    [CliOption("--data-disk-delete-option")]
    public string? DataDiskDeleteOption { get; set; }

    [CliOption("--data-disk-encryption-sets")]
    public string? DataDiskEncryptionSets { get; set; }

    [CliOption("--data-disk-iops")]
    public string? DataDiskIops { get; set; }

    [CliOption("--data-disk-mbps")]
    public string? DataDiskMbps { get; set; }

    [CliOption("--data-disk-sizes-gb")]
    public string? DataDiskSizesGb { get; set; }

    [CliFlag("--disable-integrity-monitoring-autoupgrade")]
    public bool? DisableIntegrityMonitoringAutoupgrade { get; set; }

    [CliFlag("--disable-overprovision")]
    public bool? DisableOverprovision { get; set; }

    [CliOption("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliFlag("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [CliFlag("--enable-auto-update")]
    public bool? EnableAutoUpdate { get; set; }

    [CliFlag("--enable-cross-zone-upgrade")]
    public bool? EnableCrossZoneUpgrade { get; set; }

    [CliFlag("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [CliFlag("--enable-integrity-monitoring")]
    public bool? EnableIntegrityMonitoring { get; set; }

    [CliFlag("--enable-osimage-notification")]
    public bool? EnableOsimageNotification { get; set; }

    [CliFlag("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [CliFlag("--enable-spot-restore")]
    public bool? EnableSpotRestore { get; set; }

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

    [CliOption("--health-probe")]
    public string? HealthProbe { get; set; }

    [CliOption("--host-group")]
    public string? HostGroup { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--lb")]
    public string? Lb { get; set; }

    [CliOption("--lb-nat-rule-name")]
    public string? LbNatRuleName { get; set; }

    [CliOption("--lb-sku")]
    public string? LbSku { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-batch-instance-percent")]
    public string? MaxBatchInstancePercent { get; set; }

    [CliOption("--max-price")]
    public string? MaxPrice { get; set; }

    [CliFlag("--max-surge")]
    public bool? MaxSurge { get; set; }

    [CliOption("--max-unhealthy-instance-percent")]
    public string? MaxUnhealthyInstancePercent { get; set; }

    [CliOption("--max-unhealthy-upgraded-instance-percent")]
    public string? MaxUnhealthyUpgradedInstancePercent { get; set; }

    [CliOption("--network-api-version")]
    public string? NetworkApiVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nsg")]
    public string? Nsg { get; set; }

    [CliOption("--orchestration-mode")]
    public string? OrchestrationMode { get; set; }

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

    [CliOption("--pause-time-between-batches")]
    public string? PauseTimeBetweenBatches { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-promotion-code")]
    public string? PlanPromotionCode { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--platform-fault-domain-count")]
    public int? PlatformFaultDomainCount { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliFlag("--prioritize-unhealthy-instances")]
    public bool? PrioritizeUnhealthyInstances { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CliOption("--public-ip-address-dns-name")]
    public string? PublicIpAddressDnsName { get; set; }

    [CliFlag("--public-ip-per-vm")]
    public bool? PublicIpPerVm { get; set; }

    [CliOption("--regular-priority-count")]
    public int? RegularPriorityCount { get; set; }

    [CliOption("--regular-priority-percentage")]
    public string? RegularPriorityPercentage { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scale-in-policy")]
    public string? ScaleInPolicy { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--security-type")]
    public string? SecurityType { get; set; }

    [CliFlag("--single-placement-group")]
    public bool? SinglePlacementGroup { get; set; }

    [CliFlag("--specialized")]
    public bool? Specialized { get; set; }

    [CliOption("--spot-restore-timeout")]
    public string? SpotRestoreTimeout { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

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

    [CliOption("--terminate-notification-time")]
    public string? TerminateNotificationTime { get; set; }

    [CliFlag("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CliOption("--upgrade-policy-mode")]
    public string? UpgradePolicyMode { get; set; }

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

    [CliOption("--vm-domain-name")]
    public string? VmDomainName { get; set; }

    [CliOption("--vm-sku")]
    public string? VmSku { get; set; }

    [CliOption("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}