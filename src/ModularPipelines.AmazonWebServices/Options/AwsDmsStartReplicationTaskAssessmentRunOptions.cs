using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "start-replication-task-assessment-run")]
public record AwsDmsStartReplicationTaskAssessmentRunOptions(
[property: CommandSwitch("--replication-task-arn")] string ReplicationTaskArn,
[property: CommandSwitch("--service-access-role-arn")] string ServiceAccessRoleArn,
[property: CommandSwitch("--result-location-bucket")] string ResultLocationBucket,
[property: CommandSwitch("--assessment-run-name")] string AssessmentRunName
) : AwsOptions
{
    [CommandSwitch("--result-location-folder")]
    public string? ResultLocationFolder { get; set; }

    [CommandSwitch("--result-encryption-mode")]
    public string? ResultEncryptionMode { get; set; }

    [CommandSwitch("--result-kms-key-arn")]
    public string? ResultKmsKeyArn { get; set; }

    [CommandSwitch("--include-only")]
    public string[]? IncludeOnly { get; set; }

    [CommandSwitch("--exclude")]
    public string[]? Exclude { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}