using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-replication-task-assessment")]
public record AwsDmsStartReplicationTaskAssessmentOptions(
[property: CliOption("--replication-task-arn")] string ReplicationTaskArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}