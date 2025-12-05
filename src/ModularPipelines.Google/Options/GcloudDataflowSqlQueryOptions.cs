using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "sql", "query")]
public record GcloudDataflowSqlQueryOptions(
[property: CliArgument] string Query,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--region")] string Region,
[property: CliOption("--bigquery-table")] string BigqueryTable,
[property: CliOption("--bigquery-dataset")] string BigqueryDataset,
[property: CliOption("--bigquery-project")] string BigqueryProject,
[property: CliOption("--pubsub-topic")] string PubsubTopic,
[property: CliOption("--pubsub-project")] string PubsubProject
) : GcloudOptions
{
    [CliOption("--bigquery-write-disposition")]
    public string? BigqueryWriteDisposition { get; set; }

    [CliOption("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [CliFlag("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--num-workers")]
    public string? NumWorkers { get; set; }

    [CliOption("--pubsub-create-disposition")]
    public string? PubsubCreateDisposition { get; set; }

    [CliOption("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CliOption("--parameter")]
    public string? Parameter { get; set; }

    [CliOption("--parameters-file")]
    public string? ParametersFile { get; set; }

    [CliOption("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CliOption("--worker-zone")]
    public string? WorkerZone { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}