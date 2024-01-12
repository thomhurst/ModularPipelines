using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "bulk", "create")]
public record GcloudComputeInstancesBulkCreateOptions(
[property: CommandSwitch("--name-pattern")] string NamePattern,
[property: CommandSwitch("--predefined-names")] string[] PredefinedNames,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [BooleanCommandSwitch("--no-address")]
    public bool? NoAddress { get; set; }

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

    [CommandSwitch("--count")]
    public string? Count { get; set; }

    [CommandSwitch("--create-disk")]
    public string[]? CreateDisk { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--disk")]
    public string[]? Disk { get; set; }

    [CommandSwitch("--[no-]enable-nested-virtualization")]
    public string[]? NoEnableNestedVirtualization { get; set; }

    [CommandSwitch("--[no-]enable-uefi-networking")]
    public string[]? NoEnableUefiNetworking { get; set; }

    [BooleanCommandSwitch("--erase-windows-vss-signature")]
    public bool? EraseWindowsVssSignature { get; set; }

    [CommandSwitch("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--local-ssd")]
    public string[]? LocalSsd { get; set; }

    [CommandSwitch("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CommandSwitch("--location-policy")]
    public string[]? LocationPolicy { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--max-count-per-zone")]
    public string[]? MaxCountPerZone { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CommandSwitch("--min-count")]
    public string? MinCount { get; set; }

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

    [CommandSwitch("--post-key-revocation-action-type")]
    public string? PostKeyRevocationActionType { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

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

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--target-distribution-shape")]
    public string? TargetDistributionShape { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--visible-core-count")]
    public string? VisibleCoreCount { get; set; }

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

    [CommandSwitch("--image-project")]
    public string? ImageProject { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-family")]
    public string? ImageFamily { get; set; }

    [CommandSwitch("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CommandSwitch("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [BooleanCommandSwitch("MIGRATE")]
    public bool? Migrate { get; set; }

    [BooleanCommandSwitch("TERMINATE")]
    public bool? Terminate { get; set; }

    [CommandSwitch("--on-host-maintenance")]
    public string? OnHostMaintenance { get; set; }

    [BooleanCommandSwitch("--public-dns")]
    public bool? PublicDns { get; set; }

    [BooleanCommandSwitch("--no-public-dns")]
    public bool? NoPublicDns { get; set; }

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