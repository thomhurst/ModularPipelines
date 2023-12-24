using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "update-channel-message")]
public record AwsChimeSdkMessagingUpdateChannelMessageOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--message-id")] string MessageId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}