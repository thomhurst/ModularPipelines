using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "update-findings-feedback")]
public record AwsGuarddutyUpdateFindingsFeedbackOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--finding-ids")] string[] FindingIds,
[property: CommandSwitch("--feedback")] string Feedback
) : AwsOptions
{
    [CommandSwitch("--comments")]
    public string? Comments { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}