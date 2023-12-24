using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-replication-task")]
public record AwsDmsModifyReplicationTaskOptions(
[property: CommandSwitch("--replication-task-arn")] string ReplicationTaskArn
) : AwsOptions
{
    [CommandSwitch("--replication-task-identifier")]
    public string? ReplicationTaskIdentifier { get; set; }

    [CommandSwitch("--migration-type")]
    public string? MigrationType { get; set; }

    [CommandSwitch("--table-mappings")]
    public string? TableMappings { get; set; }

    [CommandSwitch("--replication-task-settings")]
    public string? ReplicationTaskSettings { get; set; }

    [CommandSwitch("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CommandSwitch("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CommandSwitch("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CommandSwitch("--task-data")]
    public string? TaskData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}