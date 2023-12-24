using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-assessment-control")]
public record AwsAuditmanagerUpdateAssessmentControlOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--control-set-id")] string ControlSetId,
[property: CommandSwitch("--control-id")] string ControlId
) : AwsOptions
{
    [CommandSwitch("--control-status")]
    public string? ControlStatus { get; set; }

    [CommandSwitch("--comment-body")]
    public string? CommentBody { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}