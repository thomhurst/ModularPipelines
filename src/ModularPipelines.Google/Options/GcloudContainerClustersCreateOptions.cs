using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "create")]
public record GcloudContainerClustersCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliOption("--additional-zones")]
    public string[]? AdditionalZones { get; set; }

    [CliOption("--addons")]
    public string[]? Addons { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--autoprovisioning-network-tags")]
    public string[]? AutoprovisioningNetworkTags { get; set; }

    [CliOption("--autoprovisioning-resource-manager-tags")]
    public IEnumerable<KeyValue>? AutoprovisioningResourceManagerTags { get; set; }

    [CliOption("--autoscaling-profile")]
    public string? AutoscalingProfile { get; set; }

    [CliOption("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

    [CliOption("--cloud-run-config")]
    public string[]? CloudRunConfig { get; set; }

    [CliOption("--cluster-ipv4-cidr")]
    public string? ClusterIpv4Cidr { get; set; }

    [CliOption("--cluster-secondary-range-name")]
    public string? ClusterSecondaryRangeName { get; set; }

    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--create-subnetwork")]
    public IEnumerable<KeyValue>? CreateSubnetwork { get; set; }

    [CliOption("--database-encryption-key")]
    public string? DatabaseEncryptionKey { get; set; }

    [CliOption("--default-max-pods-per-node")]
    public string? DefaultMaxPodsPerNode { get; set; }

    [CliFlag("--disable-default-snat")]
    public bool? DisableDefaultSnat { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliOption("--disk-type")]
    public string? DiskType { get; set; }

    [CliFlag("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CliFlag("--enable-autoupgrade")]
    public bool? EnableAutoupgrade { get; set; }

    [CliFlag("--enable-cloud-logging")]
    public bool? EnableCloudLogging { get; set; }

    [CliFlag("--enable-cloud-monitoring")]
    public bool? EnableCloudMonitoring { get; set; }

    [CliFlag("--enable-cloud-run-alpha")]
    public bool? EnableCloudRunAlpha { get; set; }

    [CliFlag("--enable-confidential-nodes")]
    public bool? EnableConfidentialNodes { get; set; }

    [CliFlag("--enable-cost-allocation")]
    public bool? EnableCostAllocation { get; set; }

    [CliFlag("--enable-dataplane-v2")]
    public bool? EnableDataplaneV2 { get; set; }

    [CliFlag("--enable-fleet")]
    public bool? EnableFleet { get; set; }

    [CliFlag("--enable-google-cloud-access")]
    public bool? EnableGoogleCloudAccess { get; set; }

    [CliFlag("--enable-gvnic")]
    public bool? EnableGvnic { get; set; }

    [CliFlag("--enable-identity-service")]
    public bool? EnableIdentityService { get; set; }

    [CliFlag("--enable-image-streaming")]
    public bool? EnableImageStreaming { get; set; }

    [CliFlag("--enable-intra-node-visibility")]
    public bool? EnableIntraNodeVisibility { get; set; }

    [CliFlag("--enable-ip-alias")]
    public bool? EnableIpAlias { get; set; }

    [CliFlag("--enable-kubernetes-alpha")]
    public bool? EnableKubernetesAlpha { get; set; }

    [CliOption("--enable-kubernetes-unstable-apis")]
    public string[]? EnableKubernetesUnstableApis { get; set; }

    [CliFlag("--enable-l4-ilb-subsetting")]
    public bool? EnableL4IlbSubsetting { get; set; }

    [CliFlag("--enable-legacy-authorization")]
    public bool? EnableLegacyAuthorization { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CliFlag("--enable-master-global-access")]
    public bool? EnableMasterGlobalAccess { get; set; }

    [CliFlag("--enable-multi-networking")]
    public bool? EnableMultiNetworking { get; set; }

    [CliFlag("--enable-network-policy")]
    public bool? EnableNetworkPolicy { get; set; }

    [CliFlag("--enable-service-externalips")]
    public bool? EnableServiceExternalips { get; set; }

    [CliFlag("--enable-shielded-nodes")]
    public bool? EnableShieldedNodes { get; set; }

    [CliFlag("--enable-stackdriver-kubernetes")]
    public bool? EnableStackdriverKubernetes { get; set; }

    [CliFlag("--enable-vertical-pod-autoscaling")]
    public bool? EnableVerticalPodAutoscaling { get; set; }

    [CliOption("--fleet-project")]
    public string? FleetProject { get; set; }

    [CliOption("--gateway-api")]
    public string? GatewayApi { get; set; }

    [CliOption("--image-type")]
    public string? ImageType { get; set; }

    [CliOption("--in-transit-encryption")]
    public string? InTransitEncryption { get; set; }

    [CliOption("--ipv6-access-type")]
    public string? Ipv6AccessType { get; set; }

    [CliFlag("--issue-client-certificate")]
    public bool? IssueClientCertificate { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--logging-variant")]
    public string? LoggingVariant { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--max-nodes-per-pool")]
    public string? MaxNodesPerPool { get; set; }

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

    [CliOption("--monitoring")]
    public string[]? Monitoring { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-performance-configs")]
    public string[]? NetworkPerformanceConfigs { get; set; }

    [CliOption("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CliOption("--node-locations")]
    public string[]? NodeLocations { get; set; }

    [CliOption("--node-taints")]
    public string[]? NodeTaints { get; set; }

    [CliOption("--node-version")]
    public string? NodeVersion { get; set; }

    [CliOption("--notification-config")]
    public string[]? NotificationConfig { get; set; }

    [CliOption("--num-nodes")]
    public string? NumNodes { get; set; }

    [CliOption("--placement-policy")]
    public string? PlacementPolicy { get; set; }

    [CliOption("--placement-type")]
    public string? PlacementType { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--private-endpoint-subnetwork")]
    public string? PrivateEndpointSubnetwork { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CliOption("--resource-manager-tags")]
    public IEnumerable<KeyValue>? ResourceManagerTags { get; set; }

    [CliOption("--security-group")]
    public string? SecurityGroup { get; set; }

    [CliOption("--security-posture")]
    public string? SecurityPosture { get; set; }

    [CliOption("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CliOption("--services-secondary-range-name")]
    public string? ServicesSecondaryRangeName { get; set; }

    [CliFlag("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliFlag("--spot")]
    public bool? Spot { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--system-config-from-file")]
    public string? SystemConfigFromFile { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CliOption("--workload-metadata")]
    public string? WorkloadMetadata { get; set; }

    [CliOption("--workload-pool")]
    public string? WorkloadPool { get; set; }

    [CliOption("--workload-vulnerability-scanning")]
    public string? WorkloadVulnerabilityScanning { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliFlag("--enable-binauthz")]
    public bool? EnableBinauthz { get; set; }

    [CliOption("--cluster-dns")]
    public string? ClusterDns { get; set; }

    [CliFlag("clouddns")]
    public bool? Clouddns { get; set; }

    [CliFlag("default")]
    public bool? Default { get; set; }

    [CliFlag("kubedns")]
    public bool? Kubedns { get; set; }

    [CliOption("--cluster-dns-domain")]
    public string? ClusterDnsDomain { get; set; }

    [CliOption("--cluster-dns-scope")]
    public string? ClusterDnsScope { get; set; }

    [CliFlag("cluster")]
    public bool? Cluster { get; set; }

    [CliFlag("vpc")]
    public bool? Vpc { get; set; }

    [CliOption("--dataplane-v2-observability-mode")]
    public string? DataplaneV2ObservabilityMode { get; set; }

    [CliFlag("DISABLED")]
    public bool? Disabled { get; set; }

    [CliFlag("EXTERNAL_LB")]
    public bool? ExternalLb { get; set; }

    [CliFlag("INTERNAL_VPC_LB")]
    public bool? InternalVpcLb { get; set; }

    [CliFlag("--disable-dataplane-v2-flow-observability")]
    public bool? DisableDataplaneV2FlowObservability { get; set; }

    [CliFlag("--enable-dataplane-v2-flow-observability")]
    public bool? EnableDataplaneV2FlowObservability { get; set; }

    [CliFlag("--disable-dataplane-v2-metrics")]
    public bool? DisableDataplaneV2Metrics { get; set; }

    [CliFlag("--enable-dataplane-v2-metrics")]
    public bool? EnableDataplaneV2Metrics { get; set; }

    [CliFlag("--enable-autoprovisioning")]
    public bool? EnableAutoprovisioning { get; set; }

    [CliOption("--autoprovisioning-config-file")]
    public string? AutoprovisioningConfigFile { get; set; }

    [CliOption("--max-cpu")]
    public string? MaxCpu { get; set; }

    [CliOption("--max-memory")]
    public string? MaxMemory { get; set; }

    [CliOption("--autoprovisioning-image-type")]
    public string? AutoprovisioningImageType { get; set; }

    [CliOption("--autoprovisioning-locations")]
    public string[]? AutoprovisioningLocations { get; set; }

    [CliOption("--autoprovisioning-min-cpu-platform")]
    public string? AutoprovisioningMinCpuPlatform { get; set; }

    [CliOption("--min-cpu")]
    public string? MinCpu { get; set; }

    [CliOption("--min-memory")]
    public string? MinMemory { get; set; }

    [CliOption("--autoprovisioning-max-surge-upgrade")]
    public string? AutoprovisioningMaxSurgeUpgrade { get; set; }

    [CliOption("--autoprovisioning-max-unavailable-upgrade")]
    public string? AutoprovisioningMaxUnavailableUpgrade { get; set; }

    [CliOption("--autoprovisioning-node-pool-soak-duration")]
    public string? AutoprovisioningNodePoolSoakDuration { get; set; }

    [CliOption("--autoprovisioning-standard-rollout-policy")]
    public string[]? AutoprovisioningStandardRolloutPolicy { get; set; }

    [CliFlag("--enable-autoprovisioning-blue-green-upgrade")]
    public bool? EnableAutoprovisioningBlueGreenUpgrade { get; set; }

    [CliFlag("--enable-autoprovisioning-surge-upgrade")]
    public bool? EnableAutoprovisioningSurgeUpgrade { get; set; }

    [CliOption("--autoprovisioning-scopes")]
    public string[]? AutoprovisioningScopes { get; set; }

    [CliOption("--autoprovisioning-service-account")]
    public string? AutoprovisioningServiceAccount { get; set; }

    [CliFlag("--enable-autoprovisioning-autorepair")]
    public bool? EnableAutoprovisioningAutorepair { get; set; }

    [CliFlag("--enable-autoprovisioning-autoupgrade")]
    public bool? EnableAutoprovisioningAutoupgrade { get; set; }

    [CliOption("--max-accelerator")]
    public string[]? MaxAccelerator { get; set; }

    [CliFlag("type")]
    public bool? Type { get; set; }

    [CliFlag("count")]
    public bool? Count { get; set; }

    [CliOption("--min-accelerator")]
    public string[]? MinAccelerator { get; set; }

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

    [CliFlag("--enable-master-authorized-networks")]
    public bool? EnableMasterAuthorizedNetworks { get; set; }

    [CliOption("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [CliFlag("--enable-network-egress-metering")]
    public bool? EnableNetworkEgressMetering { get; set; }

    [CliFlag("--enable-resource-consumption-metering")]
    public bool? EnableResourceConsumptionMetering { get; set; }

    [CliOption("--resource-usage-bigquery-dataset")]
    public string? ResourceUsageBigqueryDataset { get; set; }

    [CliFlag("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [CliFlag("--enable-private-nodes")]
    public bool? EnablePrivateNodes { get; set; }

    [CliOption("--master-ipv4-cidr")]
    public string? MasterIpv4Cidr { get; set; }

    [CliFlag("--enable-tpu")]
    public bool? EnableTpu { get; set; }

    [CliOption("--tpu-ipv4-cidr")]
    public string? TpuIpv4Cidr { get; set; }

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

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliOption("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CliOption("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CliOption("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--enable-basic-auth")]
    public bool? EnableBasicAuth { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--reservation")]
    public string? Reservation { get; set; }

    [CliOption("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }
}