using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-signaling", "send-alexa-offer-to-master")]
public record AwsKinesisVideoSignalingSendAlexaOfferToMasterOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--sender-client-id")] string SenderClientId,
[property: CliOption("--message-payload")] string MessagePayload
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}