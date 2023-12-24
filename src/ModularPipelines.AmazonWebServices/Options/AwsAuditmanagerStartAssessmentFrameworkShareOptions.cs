using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "start-assessment-framework-share")]
public record AwsAuditmanagerStartAssessmentFrameworkShareOptions(
[property: CommandSwitch("--framework-id")] string FrameworkId,
[property: CommandSwitch("--destination-account")] string DestinationAccount,
[property: CommandSwitch("--destination-region")] string DestinationRegion
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}