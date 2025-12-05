using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "create")]
public record GcloudComputeInstancesCreateOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--boot-disk-auto-delete")]
    public bool? BootDiskAutoDelete { get; set; }

    [CliOption("--boot-disk-device-name")]
    public string? BootDiskDeviceName { get; set; }

    [CliOption("--boot-disk-provisioned-iops")]
    public string? BootDiskProvisionedIops { get; set; }

    [CliOption("--boot-disk-provisioned-throughput")]
    public string? BootDiskProvisionedThroughput { get; set; }

    [CliOption("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CliOption("--boot-disk-type")]
    public string? BootDiskType { get; set; }

    [CliFlag("--can-ip-forward")]
    public bool? CanIpForward { get; set; }

    [CliFlag("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CliOption("--create-disk")]
    public string[]? CreateDisk { get; set; }

    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliFlag("--deletion-protection")]
    public bool? DeletionProtection { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--disk")]
    public string[]? Disk { get; set; }

    [CliFlag("--enable-display-device")]
    public bool? EnableDisplayDevice { get; set; }

    [CliOption("--[no-]enable-nested-virtualization")]
    public string[]? NoEnableNestedVirtualization { get; set; }

    [CliOption("--[no-]enable-uefi-networking")]
    public string[]? NoEnableUefiNetworking { get; set; }

    [CliFlag("--erase-windows-vss-signature")]
    public bool? EraseWindowsVssSignature { get; set; }

    [CliOption("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CliOption("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CliOption("--internal-ipv6-address")]
    public string? InternalIpv6Address { get; set; }

    [CliOption("--internal-ipv6-prefix-length")]
    public string? InternalIpv6PrefixLength { get; set; }

    [CliOption("--ipv6-network-tier")]
    public string? Ipv6NetworkTier { get; set; }

    [CliOption("--ipv6-public-ptr-domain")]
    public string? Ipv6PublicPtrDomain { get; set; }

    [CliOption("--key-revocation-action-type")]
    public string? KeyRevocationActionType { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--local-ssd")]
    public string[]? LocalSsd { get; set; }

    [CliOption("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CliOption("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CliOption("--min-node-cpu")]
    public string? MinNodeCpu { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-interface")]
    public string[]? NetworkInterface { get; set; }

    [CliOption("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CliOption("--network-tier")]
    public string? NetworkTier { get; set; }

    [CliOption("--node-project")]
    public string? NodeProject { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CliOption("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [CliFlag("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CliOption("--resource-manager-tags")]
    public IEnumerable<KeyValue>? ResourceManagerTags { get; set; }

    [CliOption("--resource-policies")]
    public string[]? ResourcePolicies { get; set; }

    [CliFlag("--restart-on-failure")]
    public bool? RestartOnFailure { get; set; }

    [CliFlag("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliFlag("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CliOption("--source-instance-template")]
    public string? SourceInstanceTemplate { get; set; }

    [CliOption("--source-machine-image")]
    public string? SourceMachineImage { get; set; }

    [CliOption("--source-machine-image-csek-key-file")]
    public string? SourceMachineImageCsekKeyFile { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CliOption("--visible-core-count")]
    public string? VisibleCoreCount { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--address")]
    public string? Address { get; set; }

    [CliFlag("--no-address")]
    public bool? NoAddress { get; set; }

    [CliOption("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CliOption("--boot-disk-kms-keyring")]
    public string? BootDiskKmsKeyring { get; set; }

    [CliOption("--boot-disk-kms-location")]
    public string? BootDiskKmsLocation { get; set; }

    [CliOption("--boot-disk-kms-project")]
    public string? BootDiskKmsProject { get; set; }

    [CliOption("--custom-cpu")]
    public string? CustomCpu { get; set; }

    [CliOption("--custom-memory")]
    public string? CustomMemory { get; set; }

    [CliFlag("--custom-extensions")]
    public bool? CustomExtensions { get; set; }

    [CliOption("--custom-vm-type")]
    public string? CustomVmType { get; set; }

    [CliOption("--image-family-scope")]
    public string? ImageFamilyScope { get; set; }

    [CliOption("--image-project")]
    public string? ImageProject { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-family")]
    public string? ImageFamily { get; set; }

    [CliOption("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CliOption("--instance-kms-key")]
    public string? InstanceKmsKey { get; set; }

    [CliOption("--instance-kms-keyring")]
    public string? InstanceKmsKeyring { get; set; }

    [CliOption("--instance-kms-location")]
    public string? InstanceKmsLocation { get; set; }

    [CliOption("--instance-kms-project")]
    public string? InstanceKmsProject { get; set; }

    [CliOption("--node")]
    public string? Node { get; set; }

    [CliOption("--node-affinity-file")]
    public string? NodeAffinityFile { get; set; }

    [CliFlag("key")]
    public bool? Key { get; set; }

    [CliFlag("operator")]
    public bool? Operator { get; set; }

    [CliFlag("values")]
    public bool? Values { get; set; }

    [CliOption("--node-group")]
    public string? NodeGroup { get; set; }

    [CliFlag("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [CliFlag("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CliOption("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [CliFlag("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }

    [CliOption("--reservation")]
    public string? Reservation { get; set; }

    [CliOption("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CliFlag("any")]
    public bool? Any { get; set; }

    [CliFlag("none")]
    public bool? None { get; set; }

    [CliFlag("specific")]
    public bool? Specific { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliFlag("--no-scopes")]
    public bool? NoScopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliFlag("--no-service-account")]
    public bool? NoServiceAccount { get; set; }
}