using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-replication-task")]
public record AwsDmsStartReplicationTaskOptions(
[property: CliOption("--replication-task-arn")] string ReplicationTaskArn,
[property: CliOption("--start-replication-task-type")] string StartReplicationTaskType
) : AwsOptions
{
    [CliOption("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CliOption("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CliOption("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}