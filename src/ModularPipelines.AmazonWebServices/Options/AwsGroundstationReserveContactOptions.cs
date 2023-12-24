using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "reserve-contact")]
public record AwsGroundstationReserveContactOptions(
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--ground-station")] string GroundStation,
[property: CommandSwitch("--mission-profile-arn")] string MissionProfileArn,
[property: CommandSwitch("--satellite-arn")] string SatelliteArn,
[property: CommandSwitch("--start-time")] long StartTime
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}