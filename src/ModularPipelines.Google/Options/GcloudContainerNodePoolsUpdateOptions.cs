using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "node-pools", "update")]
public record GcloudContainerNodePoolsUpdateOptions(
[property: CliArgument] string Name,
[property: CliFlag("--enable-confidential-nodes")] bool EnableConfidentialNodes,
[property: CliFlag("--enable-gvnic")] bool EnableGvnic,
[property: CliFlag("--enable-image-streaming")] bool EnableImageStreaming,
[property: CliFlag("--enable-private-nodes")] bool EnablePrivateNodes,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels,
[property: CliOption("--logging-variant")] string LoggingVariant,
[property: CliFlag("DEFAULT")] bool Default,
[property: CliFlag("MAX_THROUGHPUT")] bool MaxThroughput,
[property: CliOption("--network-performance-configs")] string[] NetworkPerformanceConfigs,
[property: CliFlag("total-egress-bandwidth-tier")] bool TotalEgressBandwidthTier,
[property: CliOption("--node-labels")] string[] NodeLabels,
[property: CliOption("--node-locations")] string[] NodeLocations,
[property: CliOption("--node-taints")] string[] NodeTaints,
[property: CliOption("--resource-manager-tags")] IEnumerable<KeyValue> ResourceManagerTags,
[property: CliOption("--system-config-from-file")] string SystemConfigFromFile,
[property: CliOption("--tags")] string[] Tags,
[property: CliOption("--windows-os-version")] string WindowsOsVersion,
[property: CliOption("--workload-metadata")] string WorkloadMetadata,
[property: CliFlag("GCE_METADATA")] bool GceMetadata,
[property: CliFlag("GKE_METADATA")] bool GkeMetadata,
[property: CliOption("--disk-size")] string DiskSize,
[property: CliOption("--disk-type")] string DiskType,
[property: CliOption("--machine-type")] string MachineType,
[property: CliFlag("--enable-autoprovisioning")] bool EnableAutoprovisioning,
[property: CliFlag("--enable-autoscaling")] bool EnableAutoscaling,
[property: CliOption("--location-policy")] string LocationPolicy,
[property: CliOption("--max-nodes")] string MaxNodes,
[property: CliOption("--min-nodes")] string MinNodes,
[property: CliOption("--total-max-nodes")] string TotalMaxNodes,
[property: CliOption("--total-min-nodes")] string TotalMinNodes,
[property: CliFlag("--enable-autorepair")] bool EnableAutorepair,
[property: CliFlag("--enable-autoupgrade")] bool EnableAutoupgrade,
[property: CliFlag("--enable-blue-green-upgrade")] bool EnableBlueGreenUpgrade,
[property: CliFlag("--enable-surge-upgrade")] bool EnableSurgeUpgrade,
[property: CliOption("--max-surge-upgrade")] string MaxSurgeUpgrade,
[property: CliOption("--max-unavailable-upgrade")] string MaxUnavailableUpgrade,
[property: CliOption("--node-pool-soak-duration")] string NodePoolSoakDuration,
[property: CliOption("--standard-rollout-policy")] string[] StandardRolloutPolicy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}