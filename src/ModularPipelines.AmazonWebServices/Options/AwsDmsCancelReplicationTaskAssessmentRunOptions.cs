using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "cancel-replication-task-assessment-run")]
public record AwsDmsCancelReplicationTaskAssessmentRunOptions(
[property: CommandSwitch("--replication-task-assessment-run-arn")] string ReplicationTaskAssessmentRunArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}