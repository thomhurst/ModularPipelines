using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "delete-queued-messages")]
public record AwsIotwirelessDeleteQueuedMessagesOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--message-id")] string MessageId
) : AwsOptions
{
    [CliOption("--wireless-device-type")]
    public string? WirelessDeviceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}