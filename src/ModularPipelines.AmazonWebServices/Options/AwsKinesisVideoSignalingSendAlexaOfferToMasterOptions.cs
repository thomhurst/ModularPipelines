using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-signaling", "send-alexa-offer-to-master")]
public record AwsKinesisVideoSignalingSendAlexaOfferToMasterOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--sender-client-id")] string SenderClientId,
[property: CommandSwitch("--message-payload")] string MessagePayload
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}