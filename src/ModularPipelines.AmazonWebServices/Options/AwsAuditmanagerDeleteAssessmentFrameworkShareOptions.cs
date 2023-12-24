using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "delete-assessment-framework-share")]
public record AwsAuditmanagerDeleteAssessmentFrameworkShareOptions(
[property: CommandSwitch("--request-id")] string RequestId,
[property: CommandSwitch("--request-type")] string RequestType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}