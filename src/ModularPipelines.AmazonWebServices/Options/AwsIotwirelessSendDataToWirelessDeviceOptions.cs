using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "send-data-to-wireless-device")]
public record AwsIotwirelessSendDataToWirelessDeviceOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--transmit-mode")] int TransmitMode,
[property: CommandSwitch("--payload-data")] string PayloadData
) : AwsOptions
{
    [CommandSwitch("--wireless-metadata")]
    public string? WirelessMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}