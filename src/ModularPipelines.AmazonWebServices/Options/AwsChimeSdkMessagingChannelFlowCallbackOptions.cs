using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "channel-flow-callback")]
public record AwsChimeSdkMessagingChannelFlowCallbackOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--channel-message")] string ChannelMessage
) : AwsOptions
{
    [CliOption("--callback-id")]
    public string? CallbackId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}