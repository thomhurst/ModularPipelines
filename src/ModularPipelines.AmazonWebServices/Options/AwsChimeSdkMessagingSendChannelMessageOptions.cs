using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "send-channel-message")]
public record AwsChimeSdkMessagingSendChannelMessageOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--persistence")] string Persistence,
[property: CommandSwitch("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--push-notification")]
    public string? PushNotification { get; set; }

    [CommandSwitch("--message-attributes")]
    public IEnumerable<KeyValue>? MessageAttributes { get; set; }

    [CommandSwitch("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--target")]
    public string[]? Target { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}