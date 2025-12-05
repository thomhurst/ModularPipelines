using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "create")]
public record GcloudEdgeCloudContainerClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--admin-users")]
    public string? AdminUsers { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cluster-ipv4-cidr")]
    public string? ClusterIpv4Cidr { get; set; }

    [CliOption("--control-plane-kms-key")]
    public string? ControlPlaneKmsKey { get; set; }

    [CliOption("--control-plane-machine-filter")]
    public string? ControlPlaneMachineFilter { get; set; }

    [CliOption("--control-plane-node-count")]
    public string? ControlPlaneNodeCount { get; set; }

    [CliOption("--control-plane-node-location")]
    public string? ControlPlaneNodeLocation { get; set; }

    [CliOption("--control-plane-shared-deployment-policy")]
    public string? ControlPlaneSharedDeploymentPolicy { get; set; }

    [CliOption("--default-max-pods-per-node")]
    public string? DefaultMaxPodsPerNode { get; set; }

    [CliOption("--external-lb-ipv4-address-pools")]
    public string[]? ExternalLbIpv4AddressPools { get; set; }

    [CliOption("--fleet-project")]
    public string? FleetProject { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--lro-timeout")]
    public string? LroTimeout { get; set; }

    [CliOption("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CliOption("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CliOption("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }

    [CliOption("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CliOption("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CliOption("--system-addons-config")]
    public string? SystemAddonsConfig { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}