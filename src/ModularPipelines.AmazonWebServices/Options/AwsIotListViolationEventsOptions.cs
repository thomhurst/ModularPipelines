using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-violation-events")]
public record AwsIotListViolationEventsOptions(
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--thing-name")]
    public string? ThingName { get; set; }

    [CommandSwitch("--security-profile-name")]
    public string? SecurityProfileName { get; set; }

    [CommandSwitch("--behavior-criteria-type")]
    public string? BehaviorCriteriaType { get; set; }

    [CommandSwitch("--verification-state")]
    public string? VerificationState { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}