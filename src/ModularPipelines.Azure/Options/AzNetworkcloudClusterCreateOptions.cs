using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cluster", "create")]
public record AzNetworkcloudClusterCreateOptions(
[property: CommandSwitch("--aggregator-or-single-rack-definition")] string AggregatorOrSingleRackDefinition,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--cluster-version")] string ClusterVersion,
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--network-fabric-id")] string NetworkFabricId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytics-workspace-id")]
    public string? AnalyticsWorkspaceId { get; set; }

    [CommandSwitch("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CommandSwitch("--cluster-service-principal")]
    public string? ClusterServicePrincipal { get; set; }

    [CommandSwitch("--compute-deployment-threshold")]
    public string? ComputeDeploymentThreshold { get; set; }

    [CommandSwitch("--compute-rack-definitions")]
    public string? ComputeRackDefinitions { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--runtime-protection")]
    public string? RuntimeProtection { get; set; }

    [CommandSwitch("--secret-archive")]
    public string? SecretArchive { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--update-strategy")]
    public string? UpdateStrategy { get; set; }
}