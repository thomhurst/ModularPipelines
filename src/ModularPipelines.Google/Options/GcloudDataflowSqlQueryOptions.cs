using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "sql", "query")]
public record GcloudDataflowSqlQueryOptions(
[property: PositionalArgument] string Query,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--bigquery-table")] string BigqueryTable,
[property: CommandSwitch("--bigquery-dataset")] string BigqueryDataset,
[property: CommandSwitch("--bigquery-project")] string BigqueryProject,
[property: CommandSwitch("--pubsub-topic")] string PubsubTopic,
[property: CommandSwitch("--pubsub-project")] string PubsubProject
) : GcloudOptions
{
    [CommandSwitch("--bigquery-write-disposition")]
    public string? BigqueryWriteDisposition { get; set; }

    [CommandSwitch("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [BooleanCommandSwitch("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--num-workers")]
    public string? NumWorkers { get; set; }

    [CommandSwitch("--pubsub-create-disposition")]
    public string? PubsubCreateDisposition { get; set; }

    [CommandSwitch("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CommandSwitch("--parameter")]
    public string? Parameter { get; set; }

    [CommandSwitch("--parameters-file")]
    public string? ParametersFile { get; set; }

    [CommandSwitch("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CommandSwitch("--worker-zone")]
    public string? WorkerZone { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}