using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "node-pools", "create")]
public record GcloudContainerNodePoolsCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliOption("--additional-node-network")]
    public string[]? AdditionalNodeNetwork { get; set; }

    [CliOption("--additional-pod-network")]
    public string[]? AdditionalPodNetwork { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliOption("--disk-type")]
    public string? DiskType { get; set; }

    [CliFlag("--enable-autoprovisioning")]
    public bool? EnableAutoprovisioning { get; set; }

    [CliFlag("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CliFlag("--enable-autoupgrade")]
    public bool? EnableAutoupgrade { get; set; }

    [CliFlag("--enable-blue-green-upgrade")]
    public bool? EnableBlueGreenUpgrade { get; set; }

    [CliFlag("--enable-confidential-nodes")]
    public bool? EnableConfidentialNodes { get; set; }

    [CliFlag("--enable-gvnic")]
    public bool? EnableGvnic { get; set; }

    [CliFlag("--enable-image-streaming")]
    public bool? EnableImageStreaming { get; set; }

    [CliFlag("--enable-private-nodes")]
    public bool? EnablePrivateNodes { get; set; }

    [CliFlag("--enable-surge-upgrade")]
    public bool? EnableSurgeUpgrade { get; set; }

    [CliOption("--image-type")]
    public string? ImageType { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--logging-variant")]
    public string? LoggingVariant { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [CliOption("--max-surge-upgrade")]
    public string? MaxSurgeUpgrade { get; set; }

    [CliOption("--max-unavailable-upgrade")]
    public string? MaxUnavailableUpgrade { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }

    [CliOption("--min-cpu-platform")]
    public string? MinCpuPlatform { get; set; }

    [CliOption("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CliOption("--node-group")]
    public string? NodeGroup { get; set; }

    [CliOption("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CliOption("--node-locations")]
    public string[]? NodeLocations { get; set; }

    [CliOption("--node-pool-soak-duration")]
    public string? NodePoolSoakDuration { get; set; }

    [CliOption("--node-taints")]
    public string[]? NodeTaints { get; set; }

    [CliOption("--node-version")]
    public string? NodeVersion { get; set; }

    [CliOption("--num-nodes")]
    public string? NumNodes { get; set; }

    [CliOption("--placement-policy")]
    public string? PlacementPolicy { get; set; }

    [CliOption("--placement-type")]
    public string? PlacementType { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--resource-manager-tags")]
    public IEnumerable<KeyValue>? ResourceManagerTags { get; set; }

    [CliOption("--sandbox")]
    public string[]? Sandbox { get; set; }

    [CliFlag("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliOption("--sole-tenant-node-affinity-file")]
    public string? SoleTenantNodeAffinityFile { get; set; }

    [CliFlag("--spot")]
    public bool? Spot { get; set; }

    [CliOption("--standard-rollout-policy")]
    public string[]? StandardRolloutPolicy { get; set; }

    [CliOption("--system-config-from-file")]
    public string? SystemConfigFromFile { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CliOption("--tpu-topology")]
    public string? TpuTopology { get; set; }

    [CliOption("--windows-os-version")]
    public string? WindowsOsVersion { get; set; }

    [CliOption("--workload-metadata")]
    public string? WorkloadMetadata { get; set; }

    [CliOption("--create-pod-ipv4-range")]
    public IEnumerable<KeyValue>? CreatePodIpv4Range { get; set; }

    [CliOption("--pod-ipv4-range")]
    public string? PodIpv4Range { get; set; }

    [CliFlag("--enable-autoscaling")]
    public bool? EnableAutoscaling { get; set; }

    [CliOption("--location-policy")]
    public string? LocationPolicy { get; set; }

    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }

    [CliOption("--total-max-nodes")]
    public string? TotalMaxNodes { get; set; }

    [CliOption("--total-min-nodes")]
    public string? TotalMinNodes { get; set; }

    [CliFlag("--enable-best-effort-provision")]
    public bool? EnableBestEffortProvision { get; set; }

    [CliOption("--min-provision-nodes")]
    public string? MinProvisionNodes { get; set; }

    [CliOption("--ephemeral-storage-local-ssd[")]
    public string[]? EphemeralStorageLocalSsd { get; set; }

    [CliOption("--local-nvme-ssd-block[")]
    public string[]? LocalNvmeSsdBlock { get; set; }

    [CliOption("--local-ssd-count")]
    public string? LocalSsdCount { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--reservation")]
    public string? Reservation { get; set; }

    [CliOption("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }
}