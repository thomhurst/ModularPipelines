using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "get-evidence-folders-by-assessment-control")]
public record AwsAuditmanagerGetEvidenceFoldersByAssessmentControlOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--control-set-id")] string ControlSetId,
[property: CommandSwitch("--control-id")] string ControlId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}