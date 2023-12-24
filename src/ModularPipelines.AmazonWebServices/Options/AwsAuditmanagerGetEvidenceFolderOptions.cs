using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "get-evidence-folder")]
public record AwsAuditmanagerGetEvidenceFolderOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--control-set-id")] string ControlSetId,
[property: CommandSwitch("--evidence-folder-id")] string EvidenceFolderId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}