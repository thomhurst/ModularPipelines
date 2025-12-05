using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-templates", "create")]
public record GcloudComputeInstanceTemplatesCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

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

    [CliOption("--configure-disk")]
    public string[]? ConfigureDisk { get; set; }

    [CliOption("--create-disk")]
    public string[]? CreateDisk { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--disk")]
    public string[]? Disk { get; set; }

    [CliOption("--[no-]enable-nested-virtualization")]
    public string[]? NoEnableNestedVirtualization { get; set; }

    [CliOption("--[no-]enable-uefi-networking")]
    public string[]? NoEnableUefiNetworking { get; set; }

    [CliOption("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CliOption("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CliOption("--instance-template-region")]
    public string? InstanceTemplateRegion { get; set; }

    [CliOption("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CliOption("--internal-ipv6-address")]
    public string? InternalIpv6Address { get; set; }

    [CliOption("--internal-ipv6-prefix-length")]
    public string? InternalIpv6PrefixLength { get; set; }

    [CliOption("--ipv6-network-tier")]
    public string? Ipv6NetworkTier { get; set; }

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

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CliOption("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

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

    [CliOption("--source-instance")]
    public string? SourceInstance { get; set; }

    [CliOption("--source-instance-zone")]
    public string? SourceInstanceZone { get; set; }

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

    [CliOption("--image-project")]
    public string? ImageProject { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-family")]
    public string? ImageFamily { get; set; }

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

    [CliOption("--service-proxy")]
    public string[]? ServiceProxy { get; set; }

    [CliOption("--service-proxy-labels")]
    public IEnumerable<KeyValue>? ServiceProxyLabels { get; set; }
}