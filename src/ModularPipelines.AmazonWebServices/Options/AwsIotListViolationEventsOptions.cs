using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "list-violation-events")]
public record AwsIotListViolationEventsOptions(
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--thing-name")]
    public string? ThingName { get; set; }

    [CliOption("--security-profile-name")]
    public string? SecurityProfileName { get; set; }

    [CliOption("--behavior-criteria-type")]
    public string? BehaviorCriteriaType { get; set; }

    [CliOption("--verification-state")]
    public string? VerificationState { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}