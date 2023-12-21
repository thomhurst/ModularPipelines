using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks", "clusterpool", "update")]
public record AzHdinsightOnAksClusterpoolUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--cluster-pool-name")]
    public string? ClusterPoolName { get; set; }

    [CommandSwitch("--cluster-pool-version")]
    public string? ClusterPoolVersion { get; set; }

    [CommandSwitch("--compute-profile")]
    public string? ComputeProfile { get; set; }

    [BooleanCommandSwitch("--enable-log-analytics")]
    public bool? EnableLogAnalytics { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--la-workspace-id")]
    public string? LaWorkspaceId { get; set; }

    [CommandSwitch("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}