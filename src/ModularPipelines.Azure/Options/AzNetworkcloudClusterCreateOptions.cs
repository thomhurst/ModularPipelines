using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "cluster", "create")]
public record AzNetworkcloudClusterCreateOptions(
[property: CliOption("--aggregator-or-single-rack-definition")] string AggregatorOrSingleRackDefinition,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-type")] string ClusterType,
[property: CliOption("--cluster-version")] string ClusterVersion,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--network-fabric-id")] string NetworkFabricId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--analytics-workspace-id")]
    public string? AnalyticsWorkspaceId { get; set; }

    [CliOption("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CliOption("--cluster-service-principal")]
    public string? ClusterServicePrincipal { get; set; }

    [CliOption("--compute-deployment-threshold")]
    public string? ComputeDeploymentThreshold { get; set; }

    [CliOption("--compute-rack-definitions")]
    public string? ComputeRackDefinitions { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--runtime-protection")]
    public string? RuntimeProtection { get; set; }

    [CliOption("--secret-archive")]
    public string? SecretArchive { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--update-strategy")]
    public string? UpdateStrategy { get; set; }
}