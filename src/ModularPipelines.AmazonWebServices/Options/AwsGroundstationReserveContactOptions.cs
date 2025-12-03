using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "reserve-contact")]
public record AwsGroundstationReserveContactOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--ground-station")] string GroundStation,
[property: CliOption("--mission-profile-arn")] string MissionProfileArn,
[property: CliOption("--satellite-arn")] string SatelliteArn,
[property: CliOption("--start-time")] long StartTime
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}