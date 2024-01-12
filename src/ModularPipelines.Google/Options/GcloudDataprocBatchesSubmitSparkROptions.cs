using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "batches", "submit", "spark-r")]
public record GcloudDataprocBatchesSubmitSparkROptions(
[property: PositionalArgument] string MainRFile,
[property: PositionalArgument] string JobArg
) : GcloudOptions
{
    [CommandSwitch("--archives")]
    public string[]? Archives { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--batch")]
    public string? Batch { get; set; }

    [CommandSwitch("--container-image")]
    public string? ContainerImage { get; set; }

    [CommandSwitch("--deps-bucket")]
    public string? DepsBucket { get; set; }

    [CommandSwitch("--files")]
    public string[]? Files { get; set; }

    [CommandSwitch("--history-server-cluster")]
    public string? HistoryServerCluster { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--metastore-service")]
    public string? MetastoreService { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }
}