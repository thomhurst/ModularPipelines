using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-assessment-framework")]
public record AwsAuditmanagerUpdateAssessmentFrameworkOptions(
[property: CommandSwitch("--framework-id")] string FrameworkId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--control-sets")] string[] ControlSets
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--compliance-type")]
    public string? ComplianceType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}