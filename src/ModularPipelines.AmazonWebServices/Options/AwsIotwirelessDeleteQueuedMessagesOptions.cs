using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "delete-queued-messages")]
public record AwsIotwirelessDeleteQueuedMessagesOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--message-id")] string MessageId
) : AwsOptions
{
    [CommandSwitch("--wireless-device-type")]
    public string? WirelessDeviceType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}