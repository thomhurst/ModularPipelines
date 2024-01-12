using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "clusters", "gke", "create")]
public record GcloudDataprocClustersGkeCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--spark-engine-version")] string SparkEngineVersion,
[property: CommandSwitch("--gke-cluster")] string GkeCluster,
[property: CommandSwitch("--gke-cluster-location")] string GkeClusterLocation
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--pools")]
    public IEnumerable<KeyValue>? Pools { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [BooleanCommandSwitch("--setup-workload-identity")]
    public bool? SetupWorkloadIdentity { get; set; }

    [CommandSwitch("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CommandSwitch("--history-server-cluster")]
    public string? HistoryServerCluster { get; set; }

    [CommandSwitch("--history-server-cluster-region")]
    public string? HistoryServerClusterRegion { get; set; }

    [CommandSwitch("--metastore-service")]
    public string? MetastoreService { get; set; }

    [CommandSwitch("--metastore-service-location")]
    public string? MetastoreServiceLocation { get; set; }
}