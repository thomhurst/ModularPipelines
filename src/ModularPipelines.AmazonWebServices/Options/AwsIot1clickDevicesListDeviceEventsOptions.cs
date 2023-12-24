using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot1click-devices", "list-device-events")]
public record AwsIot1clickDevicesListDeviceEventsOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--from-time-stamp")] long FromTimeStamp,
[property: CommandSwitch("--to-time-stamp")] long ToTimeStamp
) : AwsOptions
{
    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}