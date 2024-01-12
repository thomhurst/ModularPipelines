using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "node-pools", "create")]
public record GcloudContainerNodePoolsCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CommandSwitch("--additional-node-network")]
    public string[]? AdditionalNodeNetwork { get; set; }

    [CommandSwitch("--additional-pod-network")]
    public string[]? AdditionalPodNetwork { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [CommandSwitch("--disk-type")]
    public string? DiskType { get; set; }

    [BooleanCommandSwitch("--enable-autoprovisioning")]
    public bool? EnableAutoprovisioning { get; set; }

    [BooleanCommandSwitch("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [BooleanCommandSwitch("--enable-autoupgrade")]
    public bool? EnableAutoupgrade { get; set; }

    [BooleanCommandSwitch("--enable-blue-green-upgrade")]
    public bool? EnableBlueGreenUpgrade { get; set; }

    [BooleanCommandSwitch("--enable-confidential-nodes")]
    public bool? EnableConfidentialNodes { get; set; }

    [BooleanCommandSwitch("--enable-gvnic")]
    public bool? EnableGvnic { get; set; }

    [BooleanCommandSwitch("--enable-image-streaming")]
    public bool? EnableImageStreaming { get; set; }

    [BooleanCommandSwitch("--enable-private-nodes")]
    public bool? EnablePrivateNodes { get; set; }

    [BooleanCommandSwitch("--enable-surge-upgrade")]
    public bool? EnableSurgeUpgrade { get; set; }

    [CommandSwitch("--image-type")]
    public string? ImageType { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--logging-variant")]
    public string? LoggingVariant { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [CommandSwitch("--max-surge-upgrade")]
    public string? MaxSurgeUpgrade { get; set; }

    [CommandSwitch("--max-unavailable-upgrade")]
    public string? MaxUnavailableUpgrade { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CommandSwitch("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CommandSwitch("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CommandSwitch("--node-group")]
    public string? NodeGroup { get; set; }

    [CommandSwitch("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CommandSwitch("--node-locations")]
    public string[]? NodeLocations { get; set; }

    [CommandSwitch("--node-pool-soak-duration")]
    public string? NodePoolSoakDuration { get; set; }

    [CommandSwitch("--node-taints")]
    public string[]? NodeTaints { get; set; }

    [CommandSwitch("--node-version")]
    public string? NodeVersion { get; set; }

    [CommandSwitch("--num-nodes")]
    public string? NumNodes { get; set; }

    [CommandSwitch("--placement-policy")]
    public string? PlacementPolicy { get; set; }

    [CommandSwitch("--placement-type")]
    public string? PlacementType { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--resource-manager-tags")]
    public IEnumerable<KeyValue>? ResourceManagerTags { get; set; }

    [CommandSwitch("--sandbox")]
    public string[]? Sandbox { get; set; }

    [BooleanCommandSwitch("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CommandSwitch("--sole-tenant-node-affinity-file")]
    public string? SoleTenantNodeAffinityFile { get; set; }

    [BooleanCommandSwitch("--spot")]
    public bool? Spot { get; set; }

    [CommandSwitch("--standard-rollout-policy")]
    public string[]? StandardRolloutPolicy { get; set; }

    [CommandSwitch("--system-config-from-file")]
    public string? SystemConfigFromFile { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--tpu-topology")]
    public string? TpuTopology { get; set; }

    [CommandSwitch("--windows-os-version")]
    public string? WindowsOsVersion { get; set; }

    [CommandSwitch("--workload-metadata")]
    public string? WorkloadMetadata { get; set; }

    [CommandSwitch("--create-pod-ipv4-range")]
    public IEnumerable<KeyValue>? CreatePodIpv4Range { get; set; }

    [CommandSwitch("--pod-ipv4-range")]
    public string? PodIpv4Range { get; set; }

    [BooleanCommandSwitch("--enable-autoscaling")]
    public bool? EnableAutoscaling { get; set; }

    [CommandSwitch("--location-policy")]
    public string? LocationPolicy { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [CommandSwitch("--total-max-nodes")]
    public string? TotalMaxNodes { get; set; }

    [CommandSwitch("--total-min-nodes")]
    public string? TotalMinNodes { get; set; }

    [BooleanCommandSwitch("--enable-best-effort-provision")]
    public bool? EnableBestEffortProvision { get; set; }

    [CommandSwitch("--min-provision-nodes")]
    public string? MinProvisionNodes { get; set; }

    [CommandSwitch("--ephemeral-storage-local-ssd[")]
    public string[]? EphemeralStorageLocalSsd { get; set; }

    [CommandSwitch("--local-nvme-ssd-block[")]
    public string[]? LocalNvmeSsdBlock { get; set; }

    [CommandSwitch("--local-ssd-count")]
    public string? LocalSsdCount { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--reservation")]
    public string? Reservation { get; set; }

    [CommandSwitch("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }
}