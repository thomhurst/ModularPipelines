using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "gke", "create")]
public record GcloudDataprocClustersGkeCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region,
[property: CliOption("--spark-engine-version")] string SparkEngineVersion,
[property: CliOption("--gke-cluster")] string GkeCluster,
[property: CliOption("--gke-cluster-location")] string GkeClusterLocation
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--pools")]
    public IEnumerable<KeyValue>? Pools { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliFlag("--setup-workload-identity")]
    public bool? SetupWorkloadIdentity { get; set; }

    [CliOption("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CliOption("--history-server-cluster")]
    public string? HistoryServerCluster { get; set; }

    [CliOption("--history-server-cluster-region")]
    public string? HistoryServerClusterRegion { get; set; }

    [CliOption("--metastore-service")]
    public string? MetastoreService { get; set; }

    [CliOption("--metastore-service-location")]
    public string? MetastoreServiceLocation { get; set; }
}