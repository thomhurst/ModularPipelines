using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "clusters", "create")]
public record GcloudContainerClustersCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CommandSwitch("--additional-zones")]
    public string[]? AdditionalZones { get; set; }

    [CommandSwitch("--addons")]
    public string[]? Addons { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--autoprovisioning-network-tags")]
    public string[]? AutoprovisioningNetworkTags { get; set; }

    [CommandSwitch("--autoprovisioning-resource-manager-tags")]
    public IEnumerable<KeyValue>? AutoprovisioningResourceManagerTags { get; set; }

    [CommandSwitch("--autoscaling-profile")]
    public string? AutoscalingProfile { get; set; }

    [CommandSwitch("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CommandSwitch("--cloud-run-config")]
    public string[]? CloudRunConfig { get; set; }

    [CommandSwitch("--cluster-ipv4-cidr")]
    public string? ClusterIpv4Cidr { get; set; }

    [CommandSwitch("--cluster-secondary-range-name")]
    public string? ClusterSecondaryRangeName { get; set; }

    [CommandSwitch("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CommandSwitch("--create-subnetwork")]
    public IEnumerable<KeyValue>? CreateSubnetwork { get; set; }

    [CommandSwitch("--database-encryption-key")]
    public string? DatabaseEncryptionKey { get; set; }

    [CommandSwitch("--default-max-pods-per-node")]
    public string? DefaultMaxPodsPerNode { get; set; }

    [BooleanCommandSwitch("--disable-default-snat")]
    public bool? DisableDefaultSnat { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [CommandSwitch("--disk-type")]
    public string? DiskType { get; set; }

    [BooleanCommandSwitch("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [BooleanCommandSwitch("--enable-autoupgrade")]
    public bool? EnableAutoupgrade { get; set; }

    [BooleanCommandSwitch("--enable-cloud-logging")]
    public bool? EnableCloudLogging { get; set; }

    [BooleanCommandSwitch("--enable-cloud-monitoring")]
    public bool? EnableCloudMonitoring { get; set; }

    [BooleanCommandSwitch("--enable-cloud-run-alpha")]
    public bool? EnableCloudRunAlpha { get; set; }

    [BooleanCommandSwitch("--enable-confidential-nodes")]
    public bool? EnableConfidentialNodes { get; set; }

    [BooleanCommandSwitch("--enable-cost-allocation")]
    public bool? EnableCostAllocation { get; set; }

    [BooleanCommandSwitch("--enable-dataplane-v2")]
    public bool? EnableDataplaneV2 { get; set; }

    [BooleanCommandSwitch("--enable-fleet")]
    public bool? EnableFleet { get; set; }

    [BooleanCommandSwitch("--enable-google-cloud-access")]
    public bool? EnableGoogleCloudAccess { get; set; }

    [BooleanCommandSwitch("--enable-gvnic")]
    public bool? EnableGvnic { get; set; }

    [BooleanCommandSwitch("--enable-identity-service")]
    public bool? EnableIdentityService { get; set; }

    [BooleanCommandSwitch("--enable-image-streaming")]
    public bool? EnableImageStreaming { get; set; }

    [BooleanCommandSwitch("--enable-intra-node-visibility")]
    public bool? EnableIntraNodeVisibility { get; set; }

    [BooleanCommandSwitch("--enable-ip-alias")]
    public bool? EnableIpAlias { get; set; }

    [BooleanCommandSwitch("--enable-kubernetes-alpha")]
    public bool? EnableKubernetesAlpha { get; set; }

    [CommandSwitch("--enable-kubernetes-unstable-apis")]
    public string[]? EnableKubernetesUnstableApis { get; set; }

    [BooleanCommandSwitch("--enable-l4-ilb-subsetting")]
    public bool? EnableL4IlbSubsetting { get; set; }

    [BooleanCommandSwitch("--enable-legacy-authorization")]
    public bool? EnableLegacyAuthorization { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [BooleanCommandSwitch("--enable-master-global-access")]
    public bool? EnableMasterGlobalAccess { get; set; }

    [BooleanCommandSwitch("--enable-multi-networking")]
    public bool? EnableMultiNetworking { get; set; }

    [BooleanCommandSwitch("--enable-network-policy")]
    public bool? EnableNetworkPolicy { get; set; }

    [BooleanCommandSwitch("--enable-service-externalips")]
    public bool? EnableServiceExternalips { get; set; }

    [BooleanCommandSwitch("--enable-shielded-nodes")]
    public bool? EnableShieldedNodes { get; set; }

    [BooleanCommandSwitch("--enable-stackdriver-kubernetes")]
    public bool? EnableStackdriverKubernetes { get; set; }

    [BooleanCommandSwitch("--enable-vertical-pod-autoscaling")]
    public bool? EnableVerticalPodAutoscaling { get; set; }

    [CommandSwitch("--fleet-project")]
    public string? FleetProject { get; set; }

    [CommandSwitch("--gateway-api")]
    public string? GatewayApi { get; set; }

    [CommandSwitch("--image-type")]
    public string? ImageType { get; set; }

    [CommandSwitch("--in-transit-encryption")]
    public string? InTransitEncryption { get; set; }

    [CommandSwitch("--ipv6-access-type")]
    public string? Ipv6AccessType { get; set; }

    [BooleanCommandSwitch("--issue-client-certificate")]
    public bool? IssueClientCertificate { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--logging-variant")]
    public string? LoggingVariant { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--max-nodes-per-pool")]
    public string? MaxNodesPerPool { get; set; }

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

    [CommandSwitch("--monitoring")]
    public string[]? Monitoring { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CommandSwitch("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CommandSwitch("--node-locations")]
    public string[]? NodeLocations { get; set; }

    [CommandSwitch("--node-taints")]
    public string[]? NodeTaints { get; set; }

    [CommandSwitch("--node-version")]
    public string? NodeVersion { get; set; }

    [CommandSwitch("--notification-config")]
    public string[]? NotificationConfig { get; set; }

    [CommandSwitch("--num-nodes")]
    public string? NumNodes { get; set; }

    [CommandSwitch("--placement-policy")]
    public string? PlacementPolicy { get; set; }

    [CommandSwitch("--placement-type")]
    public string? PlacementType { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--private-endpoint-subnetwork")]
    public string? PrivateEndpointSubnetwork { get; set; }

    [CommandSwitch("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CommandSwitch("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CommandSwitch("--resource-manager-tags")]
    public IEnumerable<KeyValue>? ResourceManagerTags { get; set; }

    [CommandSwitch("--security-group")]
    public string? SecurityGroup { get; set; }

    [CommandSwitch("--security-posture")]
    public string? SecurityPosture { get; set; }

    [CommandSwitch("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CommandSwitch("--services-secondary-range-name")]
    public string? ServicesSecondaryRangeName { get; set; }

    [BooleanCommandSwitch("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [BooleanCommandSwitch("--spot")]
    public bool? Spot { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--system-config-from-file")]
    public string? SystemConfigFromFile { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--workload-metadata")]
    public string? WorkloadMetadata { get; set; }

    [CommandSwitch("--workload-pool")]
    public string? WorkloadPool { get; set; }

    [CommandSwitch("--workload-vulnerability-scanning")]
    public string? WorkloadVulnerabilityScanning { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [BooleanCommandSwitch("--enable-binauthz")]
    public bool? EnableBinauthz { get; set; }

    [CommandSwitch("--cluster-dns")]
    public string? ClusterDns { get; set; }

    [BooleanCommandSwitch("clouddns")]
    public bool? Clouddns { get; set; }

    [BooleanCommandSwitch("default")]
    public bool? Default { get; set; }

    [BooleanCommandSwitch("kubedns")]
    public bool? Kubedns { get; set; }

    [CommandSwitch("--cluster-dns-domain")]
    public string? ClusterDnsDomain { get; set; }

    [CommandSwitch("--cluster-dns-scope")]
    public string? ClusterDnsScope { get; set; }

    [BooleanCommandSwitch("cluster")]
    public bool? Cluster { get; set; }

    [BooleanCommandSwitch("vpc")]
    public bool? Vpc { get; set; }

    [CommandSwitch("--dataplane-v2-observability-mode")]
    public string? DataplaneV2ObservabilityMode { get; set; }

    [BooleanCommandSwitch("DISABLED")]
    public bool? Disabled { get; set; }

    [BooleanCommandSwitch("EXTERNAL_LB")]
    public bool? ExternalLb { get; set; }

    [BooleanCommandSwitch("INTERNAL_VPC_LB")]
    public bool? InternalVpcLb { get; set; }

    [BooleanCommandSwitch("--disable-dataplane-v2-flow-observability")]
    public bool? DisableDataplaneV2FlowObservability { get; set; }

    [BooleanCommandSwitch("--enable-dataplane-v2-flow-observability")]
    public bool? EnableDataplaneV2FlowObservability { get; set; }

    [BooleanCommandSwitch("--disable-dataplane-v2-metrics")]
    public bool? DisableDataplaneV2Metrics { get; set; }

    [BooleanCommandSwitch("--enable-dataplane-v2-metrics")]
    public bool? EnableDataplaneV2Metrics { get; set; }

    [BooleanCommandSwitch("--enable-autoprovisioning")]
    public bool? EnableAutoprovisioning { get; set; }

    [CommandSwitch("--autoprovisioning-config-file")]
    public string? AutoprovisioningConfigFile { get; set; }

    [CommandSwitch("--max-cpu")]
    public string? MaxCpu { get; set; }

    [CommandSwitch("--max-memory")]
    public string? MaxMemory { get; set; }

    [CommandSwitch("--autoprovisioning-image-type")]
    public string? AutoprovisioningImageType { get; set; }

    [CommandSwitch("--autoprovisioning-locations")]
    public string[]? AutoprovisioningLocations { get; set; }

    [CommandSwitch("--autoprovisioning-min-cpu-platform")]
    public string? AutoprovisioningMinCpuPlatform { get; set; }

    [CommandSwitch("--min-cpu")]
    public string? MinCpu { get; set; }

    [CommandSwitch("--min-memory")]
    public string? MinMemory { get; set; }

    [CommandSwitch("--autoprovisioning-max-surge-upgrade")]
    public string? AutoprovisioningMaxSurgeUpgrade { get; set; }

    [CommandSwitch("--autoprovisioning-max-unavailable-upgrade")]
    public string? AutoprovisioningMaxUnavailableUpgrade { get; set; }

    [CommandSwitch("--autoprovisioning-node-pool-soak-duration")]
    public string? AutoprovisioningNodePoolSoakDuration { get; set; }

    [CommandSwitch("--autoprovisioning-standard-rollout-policy")]
    public string[]? AutoprovisioningStandardRolloutPolicy { get; set; }

    [BooleanCommandSwitch("--enable-autoprovisioning-blue-green-upgrade")]
    public bool? EnableAutoprovisioningBlueGreenUpgrade { get; set; }

    [BooleanCommandSwitch("--enable-autoprovisioning-surge-upgrade")]
    public bool? EnableAutoprovisioningSurgeUpgrade { get; set; }

    [CommandSwitch("--autoprovisioning-scopes")]
    public string[]? AutoprovisioningScopes { get; set; }

    [CommandSwitch("--autoprovisioning-service-account")]
    public string? AutoprovisioningServiceAccount { get; set; }

    [BooleanCommandSwitch("--enable-autoprovisioning-autorepair")]
    public bool? EnableAutoprovisioningAutorepair { get; set; }

    [BooleanCommandSwitch("--enable-autoprovisioning-autoupgrade")]
    public bool? EnableAutoprovisioningAutoupgrade { get; set; }

    [CommandSwitch("--max-accelerator")]
    public string[]? MaxAccelerator { get; set; }

    [BooleanCommandSwitch("type")]
    public bool? Type { get; set; }

    [BooleanCommandSwitch("count")]
    public bool? Count { get; set; }

    [CommandSwitch("--min-accelerator")]
    public string[]? MinAccelerator { get; set; }

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

    [BooleanCommandSwitch("--enable-master-authorized-networks")]
    public bool? EnableMasterAuthorizedNetworks { get; set; }

    [CommandSwitch("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--enable-network-egress-metering")]
    public bool? EnableNetworkEgressMetering { get; set; }

    [BooleanCommandSwitch("--enable-resource-consumption-metering")]
    public bool? EnableResourceConsumptionMetering { get; set; }

    [CommandSwitch("--resource-usage-bigquery-dataset")]
    public string? ResourceUsageBigqueryDataset { get; set; }

    [BooleanCommandSwitch("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [BooleanCommandSwitch("--enable-private-nodes")]
    public bool? EnablePrivateNodes { get; set; }

    [CommandSwitch("--master-ipv4-cidr")]
    public string? MasterIpv4Cidr { get; set; }

    [BooleanCommandSwitch("--enable-tpu")]
    public bool? EnableTpu { get; set; }

    [CommandSwitch("--tpu-ipv4-cidr")]
    public string? TpuIpv4Cidr { get; set; }

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

    [CommandSwitch("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CommandSwitch("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CommandSwitch("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CommandSwitch("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--enable-basic-auth")]
    public bool? EnableBasicAuth { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--reservation")]
    public string? Reservation { get; set; }

    [CommandSwitch("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }
}