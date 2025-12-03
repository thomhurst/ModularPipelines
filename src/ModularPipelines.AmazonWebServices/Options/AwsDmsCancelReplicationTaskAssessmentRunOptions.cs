using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "cancel-replication-task-assessment-run")]
public record AwsDmsCancelReplicationTaskAssessmentRunOptions(
[property: CliOption("--replication-task-assessment-run-arn")] string ReplicationTaskAssessmentRunArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}