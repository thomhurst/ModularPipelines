using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "start-replication-task-assessment")]
public record AwsDmsStartReplicationTaskAssessmentOptions(
[property: CommandSwitch("--replication-task-arn")] string ReplicationTaskArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}