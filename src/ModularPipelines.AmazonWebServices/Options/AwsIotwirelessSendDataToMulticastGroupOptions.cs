using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "send-data-to-multicast-group")]
public record AwsIotwirelessSendDataToMulticastGroupOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--payload-data")] string PayloadData,
[property: CommandSwitch("--wireless-metadata")] string WirelessMetadata
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}