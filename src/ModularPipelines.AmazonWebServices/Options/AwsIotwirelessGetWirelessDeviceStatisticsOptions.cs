using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-wireless-device-statistics")]
public record AwsIotwirelessGetWirelessDeviceStatisticsOptions(
[property: CommandSwitch("--wireless-device-id")] string WirelessDeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}