using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "batch-import-evidence-to-assessment-control")]
public record AwsAuditmanagerBatchImportEvidenceToAssessmentControlOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--control-set-id")] string ControlSetId,
[property: CommandSwitch("--control-id")] string ControlId,
[property: CommandSwitch("--manual-evidence")] string[] ManualEvidence
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}