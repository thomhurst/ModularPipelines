using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-wireless-device-statistics")]
public record AwsIotwirelessGetWirelessDeviceStatisticsOptions(
[property: CliOption("--wireless-device-id")] string WirelessDeviceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}