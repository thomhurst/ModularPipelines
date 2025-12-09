using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "delete-replication-task-assessment-run")]
public record AwsDmsDeleteReplicationTaskAssessmentRunOptions(
[property: CliOption("--replication-task-assessment-run-arn")] string ReplicationTaskAssessmentRunArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}