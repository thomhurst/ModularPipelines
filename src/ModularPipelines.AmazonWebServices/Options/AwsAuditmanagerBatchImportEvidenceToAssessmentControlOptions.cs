using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "batch-import-evidence-to-assessment-control")]
public record AwsAuditmanagerBatchImportEvidenceToAssessmentControlOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--control-set-id")] string ControlSetId,
[property: CliOption("--control-id")] string ControlId,
[property: CliOption("--manual-evidence")] string[] ManualEvidence
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}