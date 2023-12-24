using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-assessment-control-set-status")]
public record AwsAuditmanagerUpdateAssessmentControlSetStatusOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--control-set-id")] string ControlSetId,
[property: CommandSwitch("--status")] string Status,
[property: CommandSwitch("--comment")] string Comment
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}