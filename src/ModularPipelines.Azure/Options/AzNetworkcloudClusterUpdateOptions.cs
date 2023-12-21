using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cluster", "update")]
public record AzNetworkcloudClusterUpdateOptions : AzOptions
{
    [CommandSwitch("--aggregator-or-single-rack-definition")]
    public string? AggregatorOrSingleRackDefinition { get; set; }

    [CommandSwitch("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--cluster-service-principal")]
    public string? ClusterServicePrincipal { get; set; }

    [CommandSwitch("--compute-deployment-threshold")]
    public string? ComputeDeploymentThreshold { get; set; }

    [CommandSwitch("--compute-rack-definitions")]
    public string? ComputeRackDefinitions { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--runtime-protection")]
    public string? RuntimeProtection { get; set; }

    [CommandSwitch("--secret-archive")]
    public string? SecretArchive { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--update-strategy")]
    public string? UpdateStrategy { get; set; }
}