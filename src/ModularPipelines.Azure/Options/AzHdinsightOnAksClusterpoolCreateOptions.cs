using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks", "clusterpool", "create")]
public record AzHdinsightOnAksClusterpoolCreateOptions(
[property: CommandSwitch("--cluster-pool-name")] string ClusterPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cluster-pool-version")]
    public string? ClusterPoolVersion { get; set; }

    [BooleanCommandSwitch("--enable-log-analytics")]
    public bool? EnableLogAnalytics { get; set; }

    [CommandSwitch("--la-workspace-id")]
    public string? LaWorkspaceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workernode-size")]
    public string? WorkernodeSize { get; set; }
}