using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "send-data-to-wireless-device")]
public record AwsIotwirelessSendDataToWirelessDeviceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--transmit-mode")] int TransmitMode,
[property: CliOption("--payload-data")] string PayloadData
) : AwsOptions
{
    [CliOption("--wireless-metadata")]
    public string? WirelessMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}