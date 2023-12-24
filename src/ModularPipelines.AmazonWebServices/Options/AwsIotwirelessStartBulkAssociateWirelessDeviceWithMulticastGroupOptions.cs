using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "start-bulk-associate-wireless-device-with-multicast-group")]
public record AwsIotwirelessStartBulkAssociateWirelessDeviceWithMulticastGroupOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--query-string")]
    public string? QueryString { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}