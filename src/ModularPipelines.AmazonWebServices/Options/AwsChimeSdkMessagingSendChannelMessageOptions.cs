using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "send-channel-message")]
public record AwsChimeSdkMessagingSendChannelMessageOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--content")] string Content,
[property: CliOption("--type")] string Type,
[property: CliOption("--persistence")] string Persistence,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--push-notification")]
    public string? PushNotification { get; set; }

    [CliOption("--message-attributes")]
    public IEnumerable<KeyValue>? MessageAttributes { get; set; }

    [CliOption("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--target")]
    public string[]? Target { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}