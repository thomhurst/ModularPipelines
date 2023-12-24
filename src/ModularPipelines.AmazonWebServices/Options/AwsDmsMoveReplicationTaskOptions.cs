using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "move-replication-task")]
public record AwsDmsMoveReplicationTaskOptions(
[property: CommandSwitch("--replication-task-arn")] string ReplicationTaskArn,
[property: CommandSwitch("--target-replication-instance-arn")] string TargetReplicationInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}