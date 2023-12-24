using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "start-replication-task")]
public record AwsDmsStartReplicationTaskOptions(
[property: CommandSwitch("--replication-task-arn")] string ReplicationTaskArn,
[property: CommandSwitch("--start-replication-task-type")] string StartReplicationTaskType
) : AwsOptions
{
    [CommandSwitch("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CommandSwitch("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CommandSwitch("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}