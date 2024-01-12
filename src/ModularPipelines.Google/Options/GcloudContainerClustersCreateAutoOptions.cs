using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "clusters", "create-auto")]
public record GcloudContainerClustersCreateAutoOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--autoprovisioning-network-tags")]
    public string[]? AutoprovisioningNetworkTags { get; set; }

    [CommandSwitch("--autoprovisioning-resource-manager-tags")]
    public IEnumerable<KeyValue>? AutoprovisioningResourceManagerTags { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

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

    [BooleanCommandSwitch("--enable-backup-restore")]
    public bool? EnableBackupRestore { get; set; }

    [BooleanCommandSwitch("--enable-fleet")]
    public bool? EnableFleet { get; set; }

    [BooleanCommandSwitch("--enable-google-cloud-access")]
    public bool? EnableGoogleCloudAccess { get; set; }

    [CommandSwitch("--enable-kubernetes-unstable-apis")]
    public string[]? EnableKubernetesUnstableApis { get; set; }

    [BooleanCommandSwitch("--enable-master-global-access")]
    public bool? EnableMasterGlobalAccess { get; set; }

    [CommandSwitch("--fleet-project")]
    public string? FleetProject { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--monitoring")]
    public string[]? Monitoring { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--private-endpoint-subnetwork")]
    public string? PrivateEndpointSubnetwork { get; set; }

    [CommandSwitch("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CommandSwitch("--security-group")]
    public string? SecurityGroup { get; set; }

    [CommandSwitch("--security-posture")]
    public string? SecurityPosture { get; set; }

    [CommandSwitch("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CommandSwitch("--services-secondary-range-name")]
    public string? ServicesSecondaryRangeName { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--workload-policies")]
    public string? WorkloadPolicies { get; set; }

    [CommandSwitch("--workload-vulnerability-scanning")]
    public string? WorkloadVulnerabilityScanning { get; set; }

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

    [BooleanCommandSwitch("--enable-master-authorized-networks")]
    public bool? EnableMasterAuthorizedNetworks { get; set; }

    [CommandSwitch("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [BooleanCommandSwitch("--enable-private-nodes")]
    public bool? EnablePrivateNodes { get; set; }

    [CommandSwitch("--master-ipv4-cidr")]
    public string? MasterIpv4Cidr { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }
}