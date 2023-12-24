using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-assessment")]
public record AwsAuditmanagerUpdateAssessmentOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId,
[property: CommandSwitch("--scope")] string Scope
) : AwsOptions
{
    [CommandSwitch("--assessment-name")]
    public string? AssessmentName { get; set; }

    [CommandSwitch("--assessment-description")]
    public string? AssessmentDescription { get; set; }

    [CommandSwitch("--assessment-reports-destination")]
    public string? AssessmentReportsDestination { get; set; }

    [CommandSwitch("--roles")]
    public string[]? Roles { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}