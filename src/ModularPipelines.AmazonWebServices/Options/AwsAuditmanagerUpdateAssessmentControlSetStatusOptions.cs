using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-assessment-control-set-status")]
public record AwsAuditmanagerUpdateAssessmentControlSetStatusOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--control-set-id")] string ControlSetId,
[property: CliOption("--status")] string Status,
[property: CliOption("--comment")] string Comment
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}