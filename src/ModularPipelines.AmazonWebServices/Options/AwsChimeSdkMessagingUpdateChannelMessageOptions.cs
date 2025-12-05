using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "update-channel-message")]
public record AwsChimeSdkMessagingUpdateChannelMessageOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--message-id")] string MessageId,
[property: CliOption("--content")] string Content,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}