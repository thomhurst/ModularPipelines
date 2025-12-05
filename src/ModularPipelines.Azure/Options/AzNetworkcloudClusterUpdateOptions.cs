using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "cluster", "update")]
public record AzNetworkcloudClusterUpdateOptions : AzOptions
{
    [CliOption("--aggregator-or-single-rack-definition")]
    public string? AggregatorOrSingleRackDefinition { get; set; }

    [CliOption("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--cluster-service-principal")]
    public string? ClusterServicePrincipal { get; set; }

    [CliOption("--compute-deployment-threshold")]
    public string? ComputeDeploymentThreshold { get; set; }

    [CliOption("--compute-rack-definitions")]
    public string? ComputeRackDefinitions { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--runtime-protection")]
    public string? RuntimeProtection { get; set; }

    [CliOption("--secret-archive")]
    public string? SecretArchive { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--update-strategy")]
    public string? UpdateStrategy { get; set; }
}