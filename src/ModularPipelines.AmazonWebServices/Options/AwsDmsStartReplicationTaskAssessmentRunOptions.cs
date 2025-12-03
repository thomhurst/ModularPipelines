using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "start-replication-task-assessment-run")]
public record AwsDmsStartReplicationTaskAssessmentRunOptions(
[property: CliOption("--replication-task-arn")] string ReplicationTaskArn,
[property: CliOption("--service-access-role-arn")] string ServiceAccessRoleArn,
[property: CliOption("--result-location-bucket")] string ResultLocationBucket,
[property: CliOption("--assessment-run-name")] string AssessmentRunName
) : AwsOptions
{
    [CliOption("--result-location-folder")]
    public string? ResultLocationFolder { get; set; }

    [CliOption("--result-encryption-mode")]
    public string? ResultEncryptionMode { get; set; }

    [CliOption("--result-kms-key-arn")]
    public string? ResultKmsKeyArn { get; set; }

    [CliOption("--include-only")]
    public string[]? IncludeOnly { get; set; }

    [CliOption("--exclude")]
    public string[]? Exclude { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}