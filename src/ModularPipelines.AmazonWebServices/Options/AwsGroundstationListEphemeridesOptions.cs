using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "list-ephemerides")]
public record AwsGroundstationListEphemeridesOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--satellite-id")] string SatelliteId,
[property: CliOption("--start-time")] long StartTime
) : AwsOptions
{
    [CliOption("--status-list")]
    public string[]? StatusList { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}