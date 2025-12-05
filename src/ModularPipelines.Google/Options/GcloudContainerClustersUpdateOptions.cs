using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "update")]
public record GcloudContainerClustersUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--autoprovisioning-network-tags")] string[] AutoprovisioningNetworkTags,
[property: CliOption("--autoprovisioning-resource-manager-tags")] IEnumerable<KeyValue> AutoprovisioningResourceManagerTags,
[property: CliOption("--autoscaling-profile")] string AutoscalingProfile,
[property: CliFlag("--clear-fleet-project")] bool ClearFleetProject,
[property: CliFlag("--complete-credential-rotation")] bool CompleteCredentialRotation,
[property: CliFlag("--complete-ip-rotation")] bool CompleteIpRotation,
[property: CliOption("--database-encryption-key")] string DatabaseEncryptionKey,
[property: CliFlag("--disable-database-encryption")] bool DisableDatabaseEncryption,
[property: CliFlag("--disable-default-snat")] bool DisableDefaultSnat,
[property: CliFlag("--disable-workload-identity")] bool DisableWorkloadIdentity,
[property: CliFlag("--enable-autoscaling")] bool EnableAutoscaling,
[property: CliFlag("--enable-cost-allocation")] bool EnableCostAllocation,
[property: CliFlag("--enable-fleet")] bool EnableFleet,
[property: CliFlag("--enable-google-cloud-access")] bool EnableGoogleCloudAccess,
[property: CliFlag("--enable-identity-service")] bool EnableIdentityService,
[property: CliFlag("--enable-image-streaming")] bool EnableImageStreaming,
[property: CliFlag("--enable-intra-node-visibility")] bool EnableIntraNodeVisibility,
[property: CliOption("--enable-kubernetes-unstable-apis")] string[] EnableKubernetesUnstableApis,
[property: CliFlag("--enable-l4-ilb-subsetting")] bool EnableL4IlbSubsetting,
[property: CliFlag("--enable-legacy-authorization")] bool EnableLegacyAuthorization,
[property: CliFlag("--enable-master-authorized-networks")] bool EnableMasterAuthorizedNetworks,
[property: CliFlag("--enable-master-global-access")] bool EnableMasterGlobalAccess,
[property: CliFlag("--enable-network-policy")] bool EnableNetworkPolicy,
[property: CliFlag("--enable-private-endpoint")] bool EnablePrivateEndpoint,
[property: CliFlag("--enable-service-externalips")] bool EnableServiceExternalips,
[property: CliFlag("--enable-shielded-nodes")] bool EnableShieldedNodes,
[property: CliFlag("--enable-stackdriver-kubernetes")] bool EnableStackdriverKubernetes,
[property: CliFlag("--enable-vertical-pod-autoscaling")] bool EnableVerticalPodAutoscaling,
[property: CliOption("--fleet-project")] string FleetProject,
[property: CliOption("--gateway-api")] string GatewayApi,
[property: CliFlag("disabled")] bool Disabled,
[property: CliFlag("standard")] bool Standard,
[property: CliFlag("--generate-password")] bool GeneratePassword,
[property: CliOption("--in-transit-encryption")] string InTransitEncryption,
[property: CliOption("--logging-variant")] string LoggingVariant,
[property: CliFlag("DEFAULT")] bool Default,
[property: CliFlag("MAX_THROUGHPUT")] bool MaxThroughput,
[property: CliOption("--maintenance-window")] string MaintenanceWindow,
[property: CliOption("--network-performance-configs")] string[] NetworkPerformanceConfigs,
[property: CliFlag("total-egress-bandwidth-tier")] bool TotalEgressBandwidthTier,
[property: CliOption("--node-locations")] string[] NodeLocations,
[property: CliOption("--notification-config")] string[] NotificationConfig,
[property: CliOption("--private-ipv6-google-access-type")] string PrivateIpv6GoogleAccessType,
[property: CliOption("--release-channel")] string ReleaseChannel,
[property: CliFlag("None")] bool None,
[property: CliFlag("rapid")] bool Rapid,
[property: CliFlag("regular")] bool Regular,
[property: CliFlag("stable")] bool Stable,
[property: CliOption("--remove-labels")] string[] RemoveLabels,
[property: CliOption("--remove-workload-policies")] string RemoveWorkloadPolicies,
[property: CliOption("--security-group")] string SecurityGroup,
[property: CliOption("--security-posture")] string SecurityPosture,
[property: CliFlag("--set-password")] bool SetPassword,
[property: CliOption("--stack-type")] string StackType,
[property: CliFlag("--start-credential-rotation")] bool StartCredentialRotation,
[property: CliFlag("--start-ip-rotation")] bool StartIpRotation,
[property: CliOption("--update-addons")] string[] UpdateAddons,
[property: CliOption("--update-labels")] IEnumerable<KeyValue> UpdateLabels,
[property: CliOption("--workload-policies")] string WorkloadPolicies,
[property: CliOption("--workload-pool")] string WorkloadPool,
[property: CliOption("--workload-vulnerability-scanning")] string WorkloadVulnerabilityScanning,
[property: CliOption("--additional-pod-ipv4-ranges")] string[] AdditionalPodIpv4Ranges,
[property: CliOption("--remove-additional-pod-ipv4-ranges")] string[] RemoveAdditionalPodIpv4Ranges,
[property: CliOption("--binauthz-evaluation-mode")] string BinauthzEvaluationMode,
[property: CliFlag("--enable-binauthz")] bool EnableBinauthz,
[property: CliFlag("--clear-maintenance-window")] bool ClearMaintenanceWindow,
[property: CliOption("--remove-maintenance-exclusion")] string RemoveMaintenanceExclusion,
[property: CliOption("--add-maintenance-exclusion-end")] string AddMaintenanceExclusionEnd,
[property: CliOption("--add-maintenance-exclusion-name")] string AddMaintenanceExclusionName,
[property: CliOption("--add-maintenance-exclusion-scope")] string AddMaintenanceExclusionScope,
[property: CliOption("--add-maintenance-exclusion-start")] string AddMaintenanceExclusionStart,
[property: CliOption("--maintenance-window-end")] string MaintenanceWindowEnd,
[property: CliOption("--maintenance-window-recurrence")] string MaintenanceWindowRecurrence,
[property: CliOption("--maintenance-window-start")] string MaintenanceWindowStart,
[property: CliFlag("--clear-resource-usage-bigquery-dataset")] bool ClearResourceUsageBigqueryDataset,
[property: CliFlag("--enable-network-egress-metering")] bool EnableNetworkEgressMetering,
[property: CliFlag("--enable-resource-consumption-metering")] bool EnableResourceConsumptionMetering,
[property: CliOption("--resource-usage-bigquery-dataset")] string ResourceUsageBigqueryDataset,
[property: CliOption("--cluster-dns")] string ClusterDns,
[property: CliFlag("clouddns")] bool Clouddns,
[property: CliFlag("kubedns")] bool Kubedns,
[property: CliOption("--cluster-dns-domain")] string ClusterDnsDomain,
[property: CliOption("--cluster-dns-scope")] string ClusterDnsScope,
[property: CliFlag("cluster")] bool Cluster,
[property: CliFlag("vpc")] bool Vpc,
[property: CliOption("--dataplane-v2-observability-mode")] string DataplaneV2ObservabilityMode,
[property: CliFlag("EXTERNAL_LB")] bool ExternalLb,
[property: CliFlag("INTERNAL_VPC_LB")] bool InternalVpcLb,
[property: CliFlag("--disable-dataplane-v2-flow-observability")] bool DisableDataplaneV2FlowObservability,
[property: CliFlag("--enable-dataplane-v2-flow-observability")] bool EnableDataplaneV2FlowObservability,
[property: CliFlag("--disable-dataplane-v2-metrics")] bool DisableDataplaneV2Metrics,
[property: CliFlag("--enable-dataplane-v2-metrics")] bool EnableDataplaneV2Metrics,
[property: CliFlag("--enable-autoprovisioning")] bool EnableAutoprovisioning,
[property: CliOption("--autoprovisioning-config-file")] string AutoprovisioningConfigFile,
[property: CliOption("--autoprovisioning-image-type")] string AutoprovisioningImageType,
[property: CliOption("--autoprovisioning-locations")] string[] AutoprovisioningLocations,
[property: CliOption("--autoprovisioning-min-cpu-platform")] string AutoprovisioningMinCpuPlatform,
[property: CliOption("--max-cpu")] string MaxCpu,
[property: CliOption("--max-memory")] string MaxMemory,
[property: CliOption("--min-cpu")] string MinCpu,
[property: CliOption("--min-memory")] string MinMemory,
[property: CliOption("--autoprovisioning-max-surge-upgrade")] string AutoprovisioningMaxSurgeUpgrade,
[property: CliOption("--autoprovisioning-max-unavailable-upgrade")] string AutoprovisioningMaxUnavailableUpgrade,
[property: CliOption("--autoprovisioning-node-pool-soak-duration")] string AutoprovisioningNodePoolSoakDuration,
[property: CliOption("--autoprovisioning-standard-rollout-policy")] string[] AutoprovisioningStandardRolloutPolicy,
[property: CliFlag("--enable-autoprovisioning-blue-green-upgrade")] bool EnableAutoprovisioningBlueGreenUpgrade,
[property: CliFlag("--enable-autoprovisioning-surge-upgrade")] bool EnableAutoprovisioningSurgeUpgrade,
[property: CliOption("--autoprovisioning-scopes")] string[] AutoprovisioningScopes,
[property: CliOption("--autoprovisioning-service-account")] string AutoprovisioningServiceAccount,
[property: CliFlag("--enable-autoprovisioning-autorepair")] bool EnableAutoprovisioningAutorepair,
[property: CliFlag("--enable-autoprovisioning-autoupgrade")] bool EnableAutoprovisioningAutoupgrade,
[property: CliOption("--max-accelerator")] string[] MaxAccelerator,
[property: CliFlag("type")] bool Type,
[property: CliFlag("count")] bool Count,
[property: CliOption("--min-accelerator")] string[] MinAccelerator,
[property: CliOption("--logging")] string[] Logging,
[property: CliOption("--monitoring")] string[] Monitoring,
[property: CliFlag("--disable-managed-prometheus")] bool DisableManagedPrometheus,
[property: CliFlag("--enable-managed-prometheus")] bool EnableManagedPrometheus,
[property: CliOption("--logging-service")] string LoggingService,
[property: CliOption("--monitoring-service")] string MonitoringService,
[property: CliOption("--password")] string Password,
[property: CliFlag("--enable-basic-auth")] bool EnableBasicAuth,
[property: CliOption("--username")] string Username
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cloud-run-config")]
    public string[]? CloudRunConfig { get; set; }

    [CliOption("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [CliOption("--node-pool")]
    public string? NodePool { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

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
}