using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "get-assessment-report-url")]
public record AwsAuditmanagerGetAssessmentReportUrlOptions(
[property: CliOption("--assessment-report-id")] string AssessmentReportId,
[property: CliOption("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}