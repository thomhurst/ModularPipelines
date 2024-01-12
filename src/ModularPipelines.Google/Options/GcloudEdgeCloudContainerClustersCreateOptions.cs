using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "clusters", "create")]
public record GcloudEdgeCloudContainerClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--admin-users")]
    public string? AdminUsers { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cluster-ipv4-cidr")]
    public string? ClusterIpv4Cidr { get; set; }

    [CommandSwitch("--control-plane-kms-key")]
    public string? ControlPlaneKmsKey { get; set; }

    [CommandSwitch("--control-plane-machine-filter")]
    public string? ControlPlaneMachineFilter { get; set; }

    [CommandSwitch("--control-plane-node-count")]
    public string? ControlPlaneNodeCount { get; set; }

    [CommandSwitch("--control-plane-node-location")]
    public string? ControlPlaneNodeLocation { get; set; }

    [CommandSwitch("--control-plane-shared-deployment-policy")]
    public string? ControlPlaneSharedDeploymentPolicy { get; set; }

    [CommandSwitch("--default-max-pods-per-node")]
    public string? DefaultMaxPodsPerNode { get; set; }

    [CommandSwitch("--external-lb-ipv4-address-pools")]
    public string[]? ExternalLbIpv4AddressPools { get; set; }

    [CommandSwitch("--fleet-project")]
    public string? FleetProject { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--lro-timeout")]
    public string? LroTimeout { get; set; }

    [CommandSwitch("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CommandSwitch("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CommandSwitch("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }

    [CommandSwitch("--release-channel")]
    public string? ReleaseChannel { get; set; }

    [CommandSwitch("--services-ipv4-cidr")]
    public string? ServicesIpv4Cidr { get; set; }

    [CommandSwitch("--system-addons-config")]
    public string? SystemAddonsConfig { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}