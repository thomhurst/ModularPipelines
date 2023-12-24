using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "batch-disassociate-assessment-report-evidence")]
public record AwsAuditmanagerBatchDisassociateAssessmentReportEvidenceOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--evidence-folder-id")] string EvidenceFolderId,
[property: CommandSwitch("--evidence-ids")] string[] EvidenceIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}