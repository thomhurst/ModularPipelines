using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-assessment-control")]
public record AwsAuditmanagerUpdateAssessmentControlOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--control-set-id")] string ControlSetId,
[property: CliOption("--control-id")] string ControlId
) : AwsOptions
{
    [CliOption("--control-status")]
    public string? ControlStatus { get; set; }

    [CliOption("--comment-body")]
    public string? CommentBody { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}