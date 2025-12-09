using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-replication-task")]
public record AwsDmsCreateReplicationTaskOptions(
[property: CliOption("--replication-task-identifier")] string ReplicationTaskIdentifier,
[property: CliOption("--source-endpoint-arn")] string SourceEndpointArn,
[property: CliOption("--target-endpoint-arn")] string TargetEndpointArn,
[property: CliOption("--replication-instance-arn")] string ReplicationInstanceArn,
[property: CliOption("--migration-type")] string MigrationType,
[property: CliOption("--table-mappings")] string TableMappings
) : AwsOptions
{
    [CliOption("--replication-task-settings")]
    public string? ReplicationTaskSettings { get; set; }

    [CliOption("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CliOption("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CliOption("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--task-data")]
    public string? TaskData { get; set; }

    [CliOption("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}