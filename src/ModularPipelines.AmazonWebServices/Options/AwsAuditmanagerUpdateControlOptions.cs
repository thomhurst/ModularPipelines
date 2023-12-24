using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "update-control")]
public record AwsAuditmanagerUpdateControlOptions(
[property: CommandSwitch("--control-id")] string ControlId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--control-mapping-sources")] string[] ControlMappingSources
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--testing-information")]
    public string? TestingInformation { get; set; }

    [CommandSwitch("--action-plan-title")]
    public string? ActionPlanTitle { get; set; }

    [CommandSwitch("--action-plan-instructions")]
    public string? ActionPlanInstructions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}