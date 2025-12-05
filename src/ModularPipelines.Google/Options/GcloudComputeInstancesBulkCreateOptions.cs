using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "bulk", "create")]
public record GcloudComputeInstancesBulkCreateOptions(
[property: CliOption("--name-pattern")] string NamePattern,
[property: CliOption("--predefined-names")] string[] PredefinedNames,
[property: CliOption("--region")] string Region,
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliFlag("--no-address")]
    public bool? NoAddress { get; set; }

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

    [CliOption("--count")]
    public string? Count { get; set; }

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

    [CliFlag("--erase-windows-vss-signature")]
    public bool? EraseWindowsVssSignature { get; set; }

    [CliOption("--instance-termination-action")]
    public string? InstanceTerminationAction { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--local-ssd")]
    public string[]? LocalSsd { get; set; }

    [CliOption("--local-ssd-recovery-timeout")]
    public string? LocalSsdRecoveryTimeout { get; set; }

    [CliOption("--location-policy")]
    public string[]? LocationPolicy { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--max-count-per-zone")]
    public string[]? MaxCountPerZone { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CliOption("--min-count")]
    public string? MinCount { get; set; }

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

    [CliOption("--post-key-revocation-action-type")]
    public string? PostKeyRevocationActionType { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--provisioning-model")]
    public string? ProvisioningModel { get; set; }

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

    [CliOption("--stack-type")]
    public string? StackType { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--target-distribution-shape")]
    public string? TargetDistributionShape { get; set; }

    [CliOption("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CliOption("--visible-core-count")]
    public string? VisibleCoreCount { get; set; }

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

    [CliOption("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CliOption("--maintenance-policy")]
    public string? MaintenancePolicy { get; set; }

    [CliFlag("MIGRATE")]
    public bool? Migrate { get; set; }

    [CliFlag("TERMINATE")]
    public bool? Terminate { get; set; }

    [CliOption("--on-host-maintenance")]
    public string? OnHostMaintenance { get; set; }

    [CliFlag("--public-dns")]
    public bool? PublicDns { get; set; }

    [CliFlag("--no-public-dns")]
    public bool? NoPublicDns { get; set; }

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