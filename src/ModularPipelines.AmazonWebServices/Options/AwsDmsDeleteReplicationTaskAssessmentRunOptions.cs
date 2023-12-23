using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "delete-replication-task-assessment-run")]
public record AwsDmsDeleteReplicationTaskAssessmentRunOptions(
[property: CommandSwitch("--replication-task-assessment-run-arn")] string ReplicationTaskAssessmentRunArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}