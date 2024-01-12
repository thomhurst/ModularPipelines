using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "create")]
public record GcloudComputeInstancesCreateOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--boot-disk-auto-delete")]
    public bool? BootDiskAutoDelete { get; set; }

    [CommandSwitch("--boot-disk-device-name")]
    public string? BootDiskDeviceName { get; set; }

    [CommandSwitch("--boot-disk-provisioned-iops")]
    public string? BootDiskProvisionedIops { get; set; }

    [CommandSwitch("--boot-disk-provisioned-throughput")]
    public string? BootDiskProvisionedThroughput { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [BooleanCommandSwitch("--can-ip-forward")]
    public bool? CanIpForward { get; set; }

    [BooleanCommandSwitch("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CommandSwitch("--create-disk")]
    public string[]? CreateDisk { get; set; }

    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [BooleanCommandSwitch("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--disk")]
    public string[]? Disk { get; set; }

    [BooleanCommandSwitch("--enable-display-device")]
    public bool? EnableDisplayDevice { get; set; }

    [CommandSwitch("--[no-]enable-nested-virtualization")]
    public string[]? NoEnableNestedVirtualization { get; set; }

    [CommandSwitch("--[no-]enable-uefi-networking")]
    public string[]? NoEnableUefiNetworking { get; set; }

    [BooleanCommandSwitch("--erase-windows-vss-signature")]
    public bool? EraseWindowsVssSignature { get; set; }

    [CommandSwitch("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CommandSwitch("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CommandSwitch("--internal-ipv6-address")]
    public string? InternalIpv6Address { get; set; }

    [CommandSwitch("--internal-ipv6-prefix-length")]
    public string? InternalIpv6PrefixLength { get; set; }

    [CommandSwitch("--ipv6-network-tier")]
    public string? Ipv6NetworkTier { get; set; }

    [CommandSwitch("--ipv6-public-ptr-domain")]
    public string? Ipv6PublicPtrDomain { get; set; }

    [CommandSwitch("--key-revocation-action-type")]
    public string? KeyRevocationActionType { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--local-ssd")]
    public string[]? LocalSsd { get; set; }

    [CommandSwitch("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CommandSwitch("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CommandSwitch("--min-node-cpu")]
    public string? MinNodeCpu { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-interface")]
    public string[]? NetworkInterface { get; set; }

    [CommandSwitch("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CommandSwitch("--network-tier")]
    public string? NetworkTier { get; set; }

    [CommandSwitch("--node-project")]
    public string? NodeProject { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CommandSwitch("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CommandSwitch("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [BooleanCommandSwitch("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CommandSwitch("--resource-manager-tags")]
    public IEnumerable<KeyValue>? ResourceManagerTags { get; set; }

    [CommandSwitch("--resource-policies")]
    public string[]? ResourcePolicies { get; set; }

    [BooleanCommandSwitch("--restart-on-failure")]
    public bool? RestartOnFailure { get; set; }

    [BooleanCommandSwitch("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [BooleanCommandSwitch("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CommandSwitch("--source-instance-template")]
    public string? SourceInstanceTemplate { get; set; }

    [CommandSwitch("--source-machine-image")]
    public string? SourceMachineImage { get; set; }

    [CommandSwitch("--source-machine-image-csek-key-file")]
    public string? SourceMachineImageCsekKeyFile { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--visible-core-count")]
    public string? VisibleCoreCount { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [BooleanCommandSwitch("--no-address")]
    public bool? NoAddress { get; set; }

    [CommandSwitch("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CommandSwitch("--boot-disk-kms-keyring")]
    public string? BootDiskKmsKeyring { get; set; }

    [CommandSwitch("--boot-disk-kms-location")]
    public string? BootDiskKmsLocation { get; set; }

    [CommandSwitch("--boot-disk-kms-project")]
    public string? BootDiskKmsProject { get; set; }

    [CommandSwitch("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CommandSwitch("--custom-memory")]
    public string? CustomMemory { get; set; }

    [BooleanCommandSwitch("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CommandSwitch("--custom-vm-type")]
    public string? CustomVmType { get; set; }

    [CommandSwitch("--image-family-scope")]
    public string? ImageFamilyScope { get; set; }

    [CommandSwitch("--image-project")]
    public string? ImageProject { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-family")]
    public string? ImageFamily { get; set; }

    [CommandSwitch("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CommandSwitch("--instance-kms-key")]
    public string? InstanceKmsKey { get; set; }

    [CommandSwitch("--instance-kms-keyring")]
    public string? InstanceKmsKeyring { get; set; }

    [CommandSwitch("--instance-kms-location")]
    public string? InstanceKmsLocation { get; set; }

    [CommandSwitch("--instance-kms-project")]
    public string? InstanceKmsProject { get; set; }

    [CommandSwitch("--node")]
    public string? Node { get; set; }

    [CommandSwitch("--node-affinity-file")]
    public string? NodeAffinityFile { get; set; }

    [BooleanCommandSwitch("key")]
    public bool? Key { get; set; }

    [BooleanCommandSwitch("operator")]
    public bool? Operator { get; set; }

    [BooleanCommandSwitch("values")]
    public bool? Values { get; set; }

    [CommandSwitch("--node-group")]
    public string? NodeGroup { get; set; }

    [BooleanCommandSwitch("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [BooleanCommandSwitch("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CommandSwitch("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [BooleanCommandSwitch("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }

    [CommandSwitch("--reservation")]
    public string? Reservation { get; set; }

    [CommandSwitch("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [BooleanCommandSwitch("any")]
    public bool? Any { get; set; }

    [BooleanCommandSwitch("none")]
    public bool? None { get; set; }

    [BooleanCommandSwitch("specific")]
    public bool? Specific { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [BooleanCommandSwitch("--no-scopes")]
    public bool? NoScopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [BooleanCommandSwitch("--no-service-account")]
    public bool? NoServiceAccount { get; set; }
}