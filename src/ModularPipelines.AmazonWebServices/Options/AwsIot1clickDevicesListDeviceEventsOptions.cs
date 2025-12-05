using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-devices", "list-device-events")]
public record AwsIot1clickDevicesListDeviceEventsOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--from-time-stamp")] long FromTimeStamp,
[property: CliOption("--to-time-stamp")] long ToTimeStamp
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}