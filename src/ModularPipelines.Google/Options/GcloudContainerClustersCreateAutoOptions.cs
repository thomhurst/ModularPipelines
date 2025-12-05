using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "create-auto")]
public record GcloudContainerClustersCreateAutoOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--autoprovisioning-network-tags")]
    public string[]? AutoprovisioningNetworkTags { get; set; }

    [CliOption("--autoprovisioning-resource-manager-tags")]
    public IEnumerable<KeyValue>? AutoprovisioningResourceManagerTags { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--boot-disk-kms-key")]
    public string? BootDiskKmsKey { get; set; }

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

    [CliFlag("--enable-backup-restore")]
    public bool? EnableBackupRestore { get; set; }

    [CliFlag("--enable-fleet")]
    public bool? EnableFleet { get; set; }

    [CliFlag("--enable-google-cloud-access")]
    public bool? EnableGoogleCloudAccess { get; set; }

    [CliOption("--enable-kubernetes-unstable-apis")]
    public string[]? EnableKubernetesUnstableApis { get; set; }

    [CliFlag("--enable-master-global-access")]
    public bool? EnableMasterGlobalAccess { get; set; }

    [CliOption("--fleet-project")]
    public string? FleetProject { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--monitoring")]
    public string[]? Monitoring { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--private-endpoint-subnetwork")]
    public string? PrivateEndpointSubnetwork { get; set; }

    [CliOption("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CliOption("--security-group")]
    public string? SecurityGroup { get; set; }

    [CliOption("--security-posture")]
    public string? SecurityPosture { get; set; }

    [CliOption("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CliOption("--services-secondary-range-name")]
    public string? ServicesSecondaryRangeName { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--workload-policies")]
    public string? WorkloadPolicies { get; set; }

    [CliOption("--workload-vulnerability-scanning")]
    public string? WorkloadVulnerabilityScanning { get; set; }

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

    [CliFlag("--enable-master-authorized-networks")]
    public bool? EnableMasterAuthorizedNetworks { get; set; }

    [CliOption("--master-authorized-networks")]
    public string[]? MasterAuthorizedNetworks { get; set; }

    [CliFlag("--enable-private-endpoint")]
    public bool? EnablePrivateEndpoint { get; set; }

    [CliFlag("--enable-private-nodes")]
    public bool? EnablePrivateNodes { get; set; }

    [CliOption("--master-ipv4-cidr")]
    public string? MasterIpv4Cidr { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }
}