using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-assessment-framework-share")]
public record AwsAuditmanagerUpdateAssessmentFrameworkShareOptions(
[property: CommandSwitch("--request-id")] string RequestId,
[property: CommandSwitch("--request-type")] string RequestType,
[property: CommandSwitch("--action")] string Action
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}