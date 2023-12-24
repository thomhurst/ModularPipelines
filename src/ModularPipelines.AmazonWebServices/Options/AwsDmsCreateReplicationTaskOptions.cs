using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "create-replication-task")]
public record AwsDmsCreateReplicationTaskOptions(
[property: CommandSwitch("--replication-task-identifier")] string ReplicationTaskIdentifier,
[property: CommandSwitch("--source-endpoint-arn")] string SourceEndpointArn,
[property: CommandSwitch("--target-endpoint-arn")] string TargetEndpointArn,
[property: CommandSwitch("--replication-instance-arn")] string ReplicationInstanceArn,
[property: CommandSwitch("--migration-type")] string MigrationType,
[property: CommandSwitch("--table-mappings")] string TableMappings
) : AwsOptions
{
    [CommandSwitch("--replication-task-settings")]
    public string? ReplicationTaskSettings { get; set; }

    [CommandSwitch("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CommandSwitch("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CommandSwitch("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--task-data")]
    public string? TaskData { get; set; }

    [CommandSwitch("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}