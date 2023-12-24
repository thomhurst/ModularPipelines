using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "get-assessment-report-url")]
public record AwsAuditmanagerGetAssessmentReportUrlOptions(
[property: CommandSwitch("--assessment-report-id")] string AssessmentReportId,
[property: CommandSwitch("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}