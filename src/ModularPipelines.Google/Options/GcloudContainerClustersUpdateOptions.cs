using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "clusters", "update")]
public record GcloudContainerClustersUpdateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--autoprovisioning-network-tags")] string[] AutoprovisioningNetworkTags,
[property: CommandSwitch("--autoprovisioning-resource-manager-tags")] IEnumerable<KeyValue> AutoprovisioningResourceManagerTags,
[property: CommandSwitch("--autoscaling-profile")] string AutoscalingProfile,
[property: BooleanCommandSwitch("--clear-fleet-project")] bool ClearFleetProject,
[property: BooleanCommandSwitch("--complete-credential-rotation")] bool CompleteCredentialRotation,
[property: BooleanCommandSwitch("--complete-ip-rotation")] bool CompleteIpRotation,
[property: CommandSwitch("--database-encryption-key")] string DatabaseEncryptionKey,
[property: BooleanCommandSwitch("--disable-database-encryption")] bool DisableDatabaseEncryption,
[property: BooleanCommandSwitch("--disable-default-snat")] bool DisableDefaultSnat,
[property: BooleanCommandSwitch("--disable-workload-identity")] bool DisableWorkloadIdentity,
[property: BooleanCommandSwitch("--enable-autoscaling")] bool EnableAutoscaling,
[property: BooleanCommandSwitch("--enable-cost-allocation")] bool EnableCostAllocation,
[property: BooleanCommandSwitch("--enable-fleet")] bool EnableFleet,
[property: BooleanCommandSwitch("--enable-google-cloud-access")] bool EnableGoogleCloudAccess,
[property: BooleanCommandSwitch("--enable-identity-service")] bool EnableIdentityService,
[property: BooleanCommandSwitch("--enable-image-streaming")] bool EnableImageStreaming,
[property: BooleanCommandSwitch("--enable-intra-node-visibility")] bool EnableIntraNodeVisibility,
[property: CommandSwitch("--enable-kubernetes-unstable-apis")] string[] EnableKubernetesUnstableApis,
[property: BooleanCommandSwitch("--enable-l4-ilb-subsetting")] bool EnableL4IlbSubsetting,
[property: BooleanCommandSwitch("--enable-legacy-authorization")] bool EnableLegacyAuthorization,
[property: BooleanCommandSwitch("--enable-master-authorized-networks")] bool EnableMasterAuthorizedNetworks,
[property: BooleanCommandSwitch("--enable-master-global-access")] bool EnableMasterGlobalAccess,
[property: BooleanCommandSwitch("--enable-network-policy")] bool EnableNetworkPolicy,
[property: BooleanCommandSwitch("--enable-private-endpoint")] bool EnablePrivateEndpoint,
[property: BooleanCommandSwitch("--enable-service-externalips")] bool EnableServiceExternalips,
[property: BooleanCommandSwitch("--enable-shielded-nodes")] bool EnableShieldedNodes,
[property: BooleanCommandSwitch("--enable-stackdriver-kubernetes")] bool EnableStackdriverKubernetes,
[property: BooleanCommandSwitch("--enable-vertical-pod-autoscaling")] bool EnableVerticalPodAutoscaling,
[property: CommandSwitch("--fleet-project")] string FleetProject,
[property: CommandSwitch("--gateway-api")] string GatewayApi,
[property: BooleanCommandSwitch("disabled")] bool Disabled,
[property: BooleanCommandSwitch("standard")] bool Standard,
[property: BooleanCommandSwitch("--generate-password")] bool GeneratePassword,
[property: CommandSwitch("--in-transit-encryption")] string InTransitEncryption,
[property: CommandSwitch("--logging-variant")] string LoggingVariant,
[property: BooleanCommandSwitch("DEFAULT")] bool Default,
[property: BooleanCommandSwitch("MAX_THROUGHPUT")] bool MaxThroughput,
[property: CommandSwitch("--maintenance-window")] string MaintenanceWindow,
[property: CommandSwitch("--network-performance-configs")] string[] NetworkPerformanceConfigs,
[property: BooleanCommandSwitch("total-egress-bandwidth-tier")] bool TotalEgressBandwidthTier,
[property: CommandSwitch("--node-locations")] string[] NodeLocations,
[property: CommandSwitch("--notification-config")] string[] NotificationConfig,
[property: CommandSwitch("--private-ipv6-google-access-type")] string PrivateIpv6GoogleAccessType,
[property: CommandSwitch("--release-channel")] string ReleaseChannel,
[property: BooleanCommandSwitch("None")] bool None,
[property: BooleanCommandSwitch("rapid")] bool Rapid,
[property: BooleanCommandSwitch("regular")] bool Regular,
[property: BooleanCommandSwitch("stable")] bool Stable,
[property: CommandSwitch("--remove-labels")] string[] RemoveLabels,
[property: CommandSwitch("--remove-workload-policies")] string RemoveWorkloadPolicies,
[property: CommandSwitch("--security-group")] string SecurityGroup,
[property: CommandSwitch("--security-posture")] string SecurityPosture,
[property: BooleanCommandSwitch("--set-password")] bool SetPassword,
[property: CommandSwitch("--stack-type")] string StackType,
[property: BooleanCommandSwitch("--start-credential-rotation")] bool StartCredentialRotation,
[property: BooleanCommandSwitch("--start-ip-rotation")] bool StartIpRotation,
[property: CommandSwitch("--update-addons")] string[] UpdateAddons,
[property: CommandSwitch("--update-labels")] IEnumerable<KeyValue> UpdateLabels,
[property: CommandSwitch("--workload-policies")] string WorkloadPolicies,
[property: CommandSwitch("--workload-pool")] string WorkloadPool,
[property: CommandSwitch("--workload-vulnerability-scanning")] string WorkloadVulnerabilityScanning,
[property: CommandSwitch("--additional-pod-ipv4-ranges")] string[] AdditionalPodIpv4Ranges,
[property: CommandSwitch("--remove-additional-pod-ipv4-ranges")] string[] RemoveAdditionalPodIpv4Ranges,
[property: CommandSwitch("--binauthz-evaluation-mode")] string BinauthzEvaluationMode,
[property: BooleanCommandSwitch("--enable-binauthz")] bool EnableBinauthz,
[property: BooleanCommandSwitch("--clear-maintenance-window")] bool ClearMaintenanceWindow,
[property: CommandSwitch("--remove-maintenance-exclusion")] string RemoveMaintenanceExclusion,
[property: CommandSwitch("--add-maintenance-exclusion-end")] string AddMaintenanceExclusionEnd,
[property: CommandSwitch("--add-maintenance-exclusion-name")] string AddMaintenanceExclusionName,
[property: CommandSwitch("--add-maintenance-exclusion-scope")] string AddMaintenanceExclusionScope,
[property: CommandSwitch("--add-maintenance-exclusion-start")] string AddMaintenanceExclusionStart,
[property: CommandSwitch("--maintenance-window-end")] string MaintenanceWindowEnd,
[property: CommandSwitch("--maintenance-window-recurrence")] string MaintenanceWindowRecurrence,
[property: CommandSwitch("--maintenance-window-start")] string MaintenanceWindowStart,
[property: BooleanCommandSwitch("--clear-resource-usage-bigquery-dataset")] bool ClearResourceUsageBigqueryDataset,
[property: BooleanCommandSwitch("--enable-network-egress-metering")] bool EnableNetworkEgressMetering,
[property: BooleanCommandSwitch("--enable-resource-consumption-metering")] bool EnableResourceConsumptionMetering,
[property: CommandSwitch("--resource-usage-bigquery-dataset")] string ResourceUsageBigqueryDataset,
[property: CommandSwitch("--cluster-dns")] string ClusterDns,
[property: BooleanCommandSwitch("clouddns")] bool Clouddns,
[property: BooleanCommandSwitch("kubedns")] bool Kubedns,
[property: CommandSwitch("--cluster-dns-domain")] string ClusterDnsDomain,
[property: CommandSwitch("--cluster-dns-scope")] string ClusterDnsScope,
[property: BooleanCommandSwitch("cluster")] bool Cluster,
[property: BooleanCommandSwitch("vpc")] bool Vpc,
[property: CommandSwitch("--dataplane-v2-observability-mode")] string DataplaneV2ObservabilityMode,
[property: BooleanCommandSwitch("EXTERNAL_LB")] bool ExternalLb,
[property: BooleanCommandSwitch("INTERNAL_VPC_LB")] bool InternalVpcLb,
[property: BooleanCommandSwitch("--disable-dataplane-v2-flow-observability")] bool DisableDataplaneV2FlowObservability,
[property: BooleanCommandSwitch("--enable-dataplane-v2-flow-observability")] bool EnableDataplaneV2FlowObservability,
[property: BooleanCommandSwitch("--disable-dataplane-v2-metrics")] bool DisableDataplaneV2Metrics,
[property: BooleanCommandSwitch("--enable-dataplane-v2-metrics")] bool EnableDataplaneV2Metrics,
[property: BooleanCommandSwitch("--enable-autoprovisioning")] bool EnableAutoprovisioning,
[property: CommandSwitch("--autoprovisioning-config-file")] string AutoprovisioningConfigFile,
[property: CommandSwitch("--autoprovisioning-image-type")] string AutoprovisioningImageType,
[property: CommandSwitch("--autoprovisioning-locations")] string[] AutoprovisioningLocations,
[property: CommandSwitch("--autoprovisioning-min-cpu-platform")] string AutoprovisioningMinCpuPlatform,
[property: CommandSwitch("--max-cpu")] string MaxCpu,
[property: CommandSwitch("--max-memory")] string MaxMemory,
[property: CommandSwitch("--min-cpu")] string MinCpu,
[property: CommandSwitch("--min-memory")] string MinMemory,
[property: CommandSwitch("--autoprovisioning-max-surge-upgrade")] string AutoprovisioningMaxSurgeUpgrade,
[property: CommandSwitch("--autoprovisioning-max-unavailable-upgrade")] string AutoprovisioningMaxUnavailableUpgrade,
[property: CommandSwitch("--autoprovisioning-node-pool-soak-duration")] string AutoprovisioningNodePoolSoakDuration,
[property: CommandSwitch("--autoprovisioning-standard-rollout-policy")] string[] AutoprovisioningStandardRolloutPolicy,
[property: BooleanCommandSwitch("--enable-autoprovisioning-blue-green-upgrade")] bool EnableAutoprovisioningBlueGreenUpgrade,
[property: BooleanCommandSwitch("--enable-autoprovisioning-surge-upgrade")] bool EnableAutoprovisioningSurgeUpgrade,
[property: CommandSwitch("--autoprovisioning-scopes")] string[] AutoprovisioningScopes,
[property: CommandSwitch("--autoprovisioning-service-account")] string AutoprovisioningServiceAccount,
[property: BooleanCommandSwitch("--enable-autoprovisioning-autorepair")] bool EnableAutoprovisioningAutorepair,
[property: BooleanCommandSwitch("--enable-autoprovisioning-autoupgrade")] bool EnableAutoprovisioningAutoupgrade,
[property: CommandSwitch("--max-accelerator")] string[] MaxAccelerator,
[property: BooleanCommandSwitch("type")] bool Type,
[property: BooleanCommandSwitch("count")] bool Count,
[property: CommandSwitch("--min-accelerator")] string[] MinAccelerator,
[property: CommandSwitch("--logging")] string[] Logging,
[property: CommandSwitch("--monitoring")] string[] Monitoring,
[property: BooleanCommandSwitch("--disable-managed-prometheus")] bool DisableManagedPrometheus,
[property: BooleanCommandSwitch("--enable-managed-prometheus")] bool EnableManagedPrometheus,
[property: CommandSwitch("--logging-service")] string LoggingService,
[property: CommandSwitch("--monitoring-service")] string MonitoringService,
[property: CommandSwitch("--password")] string Password,
[property: BooleanCommandSwitch("--enable-basic-auth")] bool EnableBasicAuth,
[property: CommandSwitch("--username")] string Username
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cloud-run-config")]
    public string[]? CloudRunConfig { get; set; }

    [CommandSwitch("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [CommandSwitch("--node-pool")]
    public string? NodePool { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

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
}