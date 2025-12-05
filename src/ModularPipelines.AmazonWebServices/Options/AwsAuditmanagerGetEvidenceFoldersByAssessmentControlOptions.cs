using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "get-evidence-folders-by-assessment-control")]
public record AwsAuditmanagerGetEvidenceFoldersByAssessmentControlOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--control-set-id")] string ControlSetId,
[property: CliOption("--control-id")] string ControlId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}