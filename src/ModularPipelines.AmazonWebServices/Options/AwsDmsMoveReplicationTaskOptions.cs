using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "move-replication-task")]
public record AwsDmsMoveReplicationTaskOptions(
[property: CliOption("--replication-task-arn")] string ReplicationTaskArn,
[property: CliOption("--target-replication-instance-arn")] string TargetReplicationInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}