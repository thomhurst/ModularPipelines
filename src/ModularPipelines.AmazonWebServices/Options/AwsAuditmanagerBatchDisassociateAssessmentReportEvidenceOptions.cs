using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "batch-disassociate-assessment-report-evidence")]
public record AwsAuditmanagerBatchDisassociateAssessmentReportEvidenceOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--evidence-folder-id")] string EvidenceFolderId,
[property: CliOption("--evidence-ids")] string[] EvidenceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}