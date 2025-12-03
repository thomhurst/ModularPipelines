using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-device-position-history")]
public record AwsLocationGetDevicePositionHistoryOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CliOption("--end-time-exclusive")]
    public long? EndTimeExclusive { get; set; }

    [CliOption("--start-time-inclusive")]
    public long? StartTimeInclusive { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}