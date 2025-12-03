using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "batches", "submit", "spark-sql")]
public record GcloudDataprocBatchesSubmitSparkSqlOptions(
[property: CliArgument] string SqlScript
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--batch")]
    public string? Batch { get; set; }

    [CliOption("--container-image")]
    public string? ContainerImage { get; set; }

    [CliOption("--deps-bucket")]
    public string? DepsBucket { get; set; }

    [CliOption("--history-server-cluster")]
    public string? HistoryServerCluster { get; set; }

    [CliOption("--jars")]
    public string[]? Jars { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--metastore-service")]
    public string? MetastoreService { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliOption("--vars")]
    public string[]? Vars { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }
}