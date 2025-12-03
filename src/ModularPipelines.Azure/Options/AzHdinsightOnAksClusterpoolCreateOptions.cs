using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight-on-aks", "clusterpool", "create")]
public record AzHdinsightOnAksClusterpoolCreateOptions(
[property: CliOption("--cluster-pool-name")] string ClusterPoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cluster-pool-version")]
    public string? ClusterPoolVersion { get; set; }

    [CliFlag("--enable-log-analytics")]
    public bool? EnableLogAnalytics { get; set; }

    [CliOption("--la-workspace-id")]
    public string? LaWorkspaceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workernode-size")]
    public string? WorkernodeSize { get; set; }
}