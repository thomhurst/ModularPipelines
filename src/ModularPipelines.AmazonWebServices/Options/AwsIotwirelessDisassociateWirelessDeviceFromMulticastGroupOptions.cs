using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "disassociate-wireless-device-from-multicast-group")]
public record AwsIotwirelessDisassociateWirelessDeviceFromMulticastGroupOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--wireless-device-id")] string WirelessDeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}