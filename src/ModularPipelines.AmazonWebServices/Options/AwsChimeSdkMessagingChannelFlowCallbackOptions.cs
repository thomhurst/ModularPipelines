using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "channel-flow-callback")]
public record AwsChimeSdkMessagingChannelFlowCallbackOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--channel-message")] string ChannelMessage
) : AwsOptions
{
    [CommandSwitch("--callback-id")]
    public string? CallbackId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}