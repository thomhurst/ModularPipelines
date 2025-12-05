using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "get-evidence")]
public record AwsAuditmanagerGetEvidenceOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--control-set-id")] string ControlSetId,
[property: CliOption("--evidence-folder-id")] string EvidenceFolderId,
[property: CliOption("--evidence-id")] string EvidenceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}