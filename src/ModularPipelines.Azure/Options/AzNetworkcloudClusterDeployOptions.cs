using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cluster", "deploy")]
public record AzNetworkcloudClusterDeployOptions(
[property: CommandSwitch("--target-cluster-version")] string TargetClusterVersion
) : AzOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--skip-validations-for-machines")]
    public string? SkipValidationsForMachines { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}