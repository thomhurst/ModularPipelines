using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight-on-aks", "clusterpool", "update")]
public record AzHdinsightOnAksClusterpoolUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--cluster-pool-name")]
    public string? ClusterPoolName { get; set; }

    [CliOption("--cluster-pool-version")]
    public string? ClusterPoolVersion { get; set; }

    [CliOption("--compute-profile")]
    public string? ComputeProfile { get; set; }

    [CliFlag("--enable-log-analytics")]
    public bool? EnableLogAnalytics { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--la-workspace-id")]
    public string? LaWorkspaceId { get; set; }

    [CliOption("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}