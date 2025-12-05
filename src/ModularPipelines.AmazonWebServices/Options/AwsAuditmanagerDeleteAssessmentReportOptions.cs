using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "delete-assessment-report")]
public record AwsAuditmanagerDeleteAssessmentReportOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--assessment-report-id")] string AssessmentReportId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}