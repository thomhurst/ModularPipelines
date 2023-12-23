using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "associate-wireless-device-with-multicast-group")]
public record AwsIotwirelessAssociateWirelessDeviceWithMulticastGroupOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--wireless-device-id")] string WirelessDeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}