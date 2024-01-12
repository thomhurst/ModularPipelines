using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "node-pools", "update")]
public record GcloudContainerNodePoolsUpdateOptions(
[property: PositionalArgument] string Name,
[property: BooleanCommandSwitch("--enable-confidential-nodes")] bool EnableConfidentialNodes,
[property: BooleanCommandSwitch("--enable-gvnic")] bool EnableGvnic,
[property: BooleanCommandSwitch("--enable-image-streaming")] bool EnableImageStreaming,
[property: BooleanCommandSwitch("--enable-private-nodes")] bool EnablePrivateNodes,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels,
[property: CommandSwitch("--logging-variant")] string LoggingVariant,
[property: BooleanCommandSwitch("DEFAULT")] bool Default,
[property: BooleanCommandSwitch("MAX_THROUGHPUT")] bool MaxThroughput,
[property: CommandSwitch("--network-performance-configs")] string[] NetworkPerformanceConfigs,
[property: BooleanCommandSwitch("total-egress-bandwidth-tier")] bool TotalEgressBandwidthTier,
[property: CommandSwitch("--node-labels")] string[] NodeLabels,
[property: CommandSwitch("--node-locations")] string[] NodeLocations,
[property: CommandSwitch("--node-taints")] string[] NodeTaints,
[property: CommandSwitch("--resource-manager-tags")] IEnumerable<KeyValue> ResourceManagerTags,
[property: CommandSwitch("--system-config-from-file")] string SystemConfigFromFile,
[property: CommandSwitch("--tags")] string[] Tags,
[property: CommandSwitch("--windows-os-version")] string WindowsOsVersion,
[property: CommandSwitch("--workload-metadata")] string WorkloadMetadata,
[property: BooleanCommandSwitch("GCE_METADATA")] bool GceMetadata,
[property: BooleanCommandSwitch("GKE_METADATA")] bool GkeMetadata,
[property: CommandSwitch("--disk-size")] string DiskSize,
[property: CommandSwitch("--disk-type")] string DiskType,
[property: CommandSwitch("--machine-type")] string MachineType,
[property: BooleanCommandSwitch("--enable-autoprovisioning")] bool EnableAutoprovisioning,
[property: BooleanCommandSwitch("--enable-autoscaling")] bool EnableAutoscaling,
[property: CommandSwitch("--location-policy")] string LocationPolicy,
[property: CommandSwitch("--max-nodes")] string MaxNodes,
[property: CommandSwitch("--min-nodes")] string MinNodes,
[property: CommandSwitch("--total-max-nodes")] string TotalMaxNodes,
[property: CommandSwitch("--total-min-nodes")] string TotalMinNodes,
[property: BooleanCommandSwitch("--enable-autorepair")] bool EnableAutorepair,
[property: BooleanCommandSwitch("--enable-autoupgrade")] bool EnableAutoupgrade,
[property: BooleanCommandSwitch("--enable-blue-green-upgrade")] bool EnableBlueGreenUpgrade,
[property: BooleanCommandSwitch("--enable-surge-upgrade")] bool EnableSurgeUpgrade,
[property: CommandSwitch("--max-surge-upgrade")] string MaxSurgeUpgrade,
[property: CommandSwitch("--max-unavailable-upgrade")] string MaxUnavailableUpgrade,
[property: CommandSwitch("--node-pool-soak-duration")] string NodePoolSoakDuration,
[property: CommandSwitch("--standard-rollout-policy")] string[] StandardRolloutPolicy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}