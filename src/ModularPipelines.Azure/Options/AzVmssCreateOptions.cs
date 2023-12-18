using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "create")]
public record AzVmssCreateOptions(
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

    [CommandSwitch("--app-gateway")]
    public string? AppGateway { get; set; }

    [CommandSwitch("--app-gateway-capacity")]
    public string? AppGatewayCapacity { get; set; }

    [CommandSwitch("--app-gateway-sku")]
    public string? AppGatewaySku { get; set; }

    [CommandSwitch("--app-gateway-subnet-address-prefix")]
    public string? AppGatewaySubnetAddressPrefix { get; set; }

    [CommandSwitch("--asgs")]
    public string? Asgs { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--automatic-repairs-action")]
    public string? AutomaticRepairsAction { get; set; }

    [CommandSwitch("--automatic-repairs-grace-period")]
    public string? AutomaticRepairsGracePeriod { get; set; }

    [CommandSwitch("--backend-pool-name")]
    public string? BackendPoolName { get; set; }

    [CommandSwitch("--backend-port")]
    public string? BackendPort { get; set; }

    [CommandSwitch("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CommandSwitch("--computer-name-prefix")]
    public string? ComputerNamePrefix { get; set; }

    [CommandSwitch("--custom-data")]
    public string? CustomData { get; set; }

    [CommandSwitch("--data-disk-caching")]
    public string? DataDiskCaching { get; set; }

    [CommandSwitch("--data-disk-delete-option")]
    public string? DataDiskDeleteOption { get; set; }

    [CommandSwitch("--data-disk-encryption-sets")]
    public string? DataDiskEncryptionSets { get; set; }

    [CommandSwitch("--data-disk-iops")]
    public string? DataDiskIops { get; set; }

    [CommandSwitch("--data-disk-mbps")]
    public string? DataDiskMbps { get; set; }

    [CommandSwitch("--data-disk-sizes-gb")]
    public string? DataDiskSizesGb { get; set; }

    [BooleanCommandSwitch("--disable-integrity-monitoring-autoupgrade")]
    public bool? DisableIntegrityMonitoringAutoupgrade { get; set; }

    [BooleanCommandSwitch("--disable-overprovision")]
    public bool? DisableOverprovision { get; set; }

    [CommandSwitch("--disk-controller-type")]
    public string? DiskControllerType { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [BooleanCommandSwitch("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [BooleanCommandSwitch("--enable-auto-update")]
    public bool? EnableAutoUpdate { get; set; }

    [BooleanCommandSwitch("--enable-cross-zone-upgrade")]
    public bool? EnableCrossZoneUpgrade { get; set; }

    [BooleanCommandSwitch("--enable-hibernation")]
    public bool? EnableHibernation { get; set; }

    [BooleanCommandSwitch("--enable-integrity-monitoring")]
    public bool? EnableIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--enable-osimage-notification")]
    public bool? EnableOsimageNotification { get; set; }

    [BooleanCommandSwitch("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [BooleanCommandSwitch("--enable-spot-restore")]
    public bool? EnableSpotRestore { get; set; }

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

    [CommandSwitch("--health-probe")]
    public string? HealthProbe { get; set; }

    [CommandSwitch("--host-group")]
    public string? HostGroup { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--lb")]
    public string? Lb { get; set; }

    [CommandSwitch("--lb-nat-rule-name")]
    public string? LbNatRuleName { get; set; }

    [CommandSwitch("--lb-sku")]
    public string? LbSku { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-batch-instance-percent")]
    public string? MaxBatchInstancePercent { get; set; }

    [CommandSwitch("--max-price")]
    public string? MaxPrice { get; set; }

    [BooleanCommandSwitch("--max-surge")]
    public bool? MaxSurge { get; set; }

    [CommandSwitch("--max-unhealthy-instance-percent")]
    public string? MaxUnhealthyInstancePercent { get; set; }

    [CommandSwitch("--max-unhealthy-upgraded-instance-percent")]
    public string? MaxUnhealthyUpgradedInstancePercent { get; set; }

    [CommandSwitch("--network-api-version")]
    public string? NetworkApiVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nsg")]
    public string? Nsg { get; set; }

    [CommandSwitch("--orchestration-mode")]
    public string? OrchestrationMode { get; set; }

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

    [CommandSwitch("--pause-time-between-batches")]
    public string? PauseTimeBetweenBatches { get; set; }

    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-promotion-code")]
    public string? PlanPromotionCode { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--platform-fault-domain-count")]
    public int? PlatformFaultDomainCount { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [BooleanCommandSwitch("--prioritize-unhealthy-instances")]
    public bool? PrioritizeUnhealthyInstances { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CommandSwitch("--public-ip-address-dns-name")]
    public string? PublicIpAddressDnsName { get; set; }

    [BooleanCommandSwitch("--public-ip-per-vm")]
    public bool? PublicIpPerVm { get; set; }

    [CommandSwitch("--regular-priority-count")]
    public int? RegularPriorityCount { get; set; }

    [CommandSwitch("--regular-priority-percentage")]
    public string? RegularPriorityPercentage { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scale-in-policy")]
    public string? ScaleInPolicy { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--security-type")]
    public string? SecurityType { get; set; }

    [BooleanCommandSwitch("--single-placement-group")]
    public bool? SinglePlacementGroup { get; set; }

    [BooleanCommandSwitch("--specialized")]
    public bool? Specialized { get; set; }

    [CommandSwitch("--spot-restore-timeout")]
    public string? SpotRestoreTimeout { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

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

    [CommandSwitch("--terminate-notification-time")]
    public string? TerminateNotificationTime { get; set; }

    [BooleanCommandSwitch("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CommandSwitch("--upgrade-policy-mode")]
    public string? UpgradePolicyMode { get; set; }

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

    [CommandSwitch("--vm-domain-name")]
    public string? VmDomainName { get; set; }

    [CommandSwitch("--vm-sku")]
    public string? VmSku { get; set; }

    [CommandSwitch("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}