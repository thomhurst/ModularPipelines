using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "list-contacts")]
public record AwsGroundstationListContactsOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--status-list")] string[] StatusList
) : AwsOptions
{
    [CliOption("--ground-station")]
    public string? GroundStation { get; set; }

    [CliOption("--mission-profile-arn")]
    public string? MissionProfileArn { get; set; }

    [CliOption("--satellite-arn")]
    public string? SatelliteArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}