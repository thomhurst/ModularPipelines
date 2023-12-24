using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "list-contacts")]
public record AwsGroundstationListContactsOptions(
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--status-list")] string[] StatusList
) : AwsOptions
{
    [CommandSwitch("--ground-station")]
    public string? GroundStation { get; set; }

    [CommandSwitch("--mission-profile-arn")]
    public string? MissionProfileArn { get; set; }

    [CommandSwitch("--satellite-arn")]
    public string? SatelliteArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}