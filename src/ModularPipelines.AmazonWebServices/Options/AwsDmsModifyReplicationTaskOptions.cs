using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-replication-task")]
public record AwsDmsModifyReplicationTaskOptions(
[property: CliOption("--replication-task-arn")] string ReplicationTaskArn
) : AwsOptions
{
    [CliOption("--replication-task-identifier")]
    public string? ReplicationTaskIdentifier { get; set; }

    [CliOption("--migration-type")]
    public string? MigrationType { get; set; }

    [CliOption("--table-mappings")]
    public string? TableMappings { get; set; }

    [CliOption("--replication-task-settings")]
    public string? ReplicationTaskSettings { get; set; }

    [CliOption("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CliOption("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CliOption("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CliOption("--task-data")]
    public string? TaskData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}