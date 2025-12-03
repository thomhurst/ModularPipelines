using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "associate-assessment-report-evidence-folder")]
public record AwsAuditmanagerAssociateAssessmentReportEvidenceFolderOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--evidence-folder-id")] string EvidenceFolderId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}